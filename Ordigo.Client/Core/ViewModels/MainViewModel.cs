using MaterialDesignThemes.Wpf;
using NLog;
using Ordigo.API.Contracts;
using Ordigo.API.Model;
using Ordigo.Client.Controls.Dialogs;
using Ordigo.Client.Core.UI;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ordigo.Client.Core.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// Создает модель отображения <see cref="MainViewModel"/>
        /// </summary>
        /// <param name="client">Класс, реализующий работу с API</param>
        /// <param name="messageQueue">Очередь сообщений</param>
        public MainViewModel(IApiClient client, ISnackbarMessageQueue messageQueue)
        {
            apiClient = client;
            MessageQueue = messageQueue;

            Items = new ObservableCollection<TextNoteViewModel>();

            AddTextNoteCommand = new RelayCommand(AddTextNote, CanOpenDialog);
            AddToDoNoteCommand = new RelayCommand(AddToDoNote, CanOpenDialog);

            ChangeThemeCommand = new RelayCommand(ChangeTheme);

            SyncAllNotes = new RelayCommand(() => Sync());
            SyncArchivedNotes = new RelayCommand(() => Sync(NoteState.Archived));
            ExitCommand = new RelayCommand(Exit);

            // CS4014: Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
#pragma warning disable CS4014
            Sync();
            RunBackgroundTask(TimeSpan.FromSeconds(30), tokenSource.Token);
#pragma warning restore CS4014
        }

        #endregion

        #region Fields

        /// <inheritdoc />
        /// <summary>
        /// Событие, оповещающее об измении какого-либо свойства в модели
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// API для работы с авторизацией
        /// </summary>
        private readonly IApiClient apiClient;

        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Указывает, открыт ли какой либо из диалогов <see cref="DialogHost"/>
        /// <para>Необходимая переменная т.к. больше одного <see cref="DialogHost"/> открывать нельзя</para>
        /// </summary>
        private bool isDialogOpened;

        /// <summary>
        /// Заглушка для thread-safe доступа
        /// </summary>
        private readonly object syncLock = new object();

        /// <summary>
        /// Токен отмены операции для асинхронных задач
        /// </summary>
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        #endregion

        #region Properties

        /// <summary>
        /// Коллекция, содержащая модели отображения заметок для интерфейса
        /// </summary>
        public ObservableCollection<TextNoteViewModel> Items { get; set; }

        /// <summary>
        /// Указывает, отображается ли индикатор загрузки
        /// </summary>
        [AlsoNotifyFor(nameof(CloudIconVisibility))]
        public bool SyncAnimation { get; set; }

        /// <summary>
        /// Задает состояние отображения строки об отсутствии заметок
        /// </summary>
        public Visibility CenterLabelVisibility { get; set; }

        /// <summary>
        /// Задает состояние отображения иконки завершения синхронизации
        /// </summary>
        [DoNotNotify]
        public Visibility CloudIconVisibility
        {
            get
            {
                if (SyncAnimation)
                    return Visibility.Hidden;

                return Visibility.Visible;
            }
        }

        /// <summary>
        /// Текущая тема интерфейса
        /// </summary>
        public ColorZoneMode Theme { get; set; }

        /// <summary>
        /// Команда создания текстовой заметки
        /// </summary>
        [DoNotNotify]
        public ICommand AddTextNoteCommand { get; }

        /// <summary>
        /// Команда создания заметки с чек-листом
        /// </summary>
        [DoNotNotify]
        public ICommand AddToDoNoteCommand { get; }

        /// <summary>
        /// Команда смены темы интерфейса
        /// </summary>
        [DoNotNotify]
        public ICommand ChangeThemeCommand { get; }

        /// <summary>
        /// Команда для получения всех заметок
        /// </summary>
        public ICommand SyncAllNotes { get; }

        /// <summary>
        /// Команда для получения архивированых заметок
        /// </summary>
        public ICommand SyncArchivedNotes { get; }

        /// <summary>
        /// Команда выхода из аккаунта
        /// </summary>
        [DoNotNotify]
        public ICommand ExitCommand { get; }

        /// <summary>
        /// Очередь сообщений для снэкбара
        /// </summary>
        [DoNotNotify]
        public ISnackbarMessageQueue MessageQueue { get; }

        /// <summary>
        /// Указывает, открыт ли какой либо из диалогов <see cref="DialogHost"/>
        /// <para>Необходимая переменная т.к. больше одного <see cref="DialogHost"/> открывать нельзя</para>
        /// <para>Потокобезопасная переменная</para>
        /// </summary>
        private bool IsDialogOpened
        {
            get
            {
                lock (syncLock)
                {
                    return isDialogOpened;
                }
            }
            set
            {
                lock (syncLock)
                {
                    isDialogOpened = value;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Указывает, можно ли открыть <see cref="DialogHost"/> и выполнить команду
        /// </summary>
        /// <returns></returns>
        private bool CanOpenDialog()
        {
            if (IsDialogOpened)
                return false;

            return true;
        }

        /// <summary>
        /// Открывает диалог и создает новую заметку
        /// </summary>
        private async void AddTextNote()
        {
            var viewModel = new TextNoteViewModel();

            var dialog = new AddEditTextNoteDialog();
            var result = await OpenDialog(dialog);

            if (result)
            {
                dialog.UpdateViewModel(viewModel);

                await AddNote(viewModel);
            }
        }

        /// <summary>
        /// Открывает диалог и создает новую заметку
        /// </summary>
        private async void AddToDoNote()
        {
            var viewModel = new TextNoteViewModel { Title = "Заметка с контрольным список", Content = "1.\n2.\n3.\n" };
            var dialog = new AddEditTextNoteDialog(viewModel);

            var result = await OpenDialog(dialog);

            if (result)
            {
                dialog.UpdateViewModel(viewModel);

                await AddNote(viewModel);
            }
        }

        /// <summary>
        /// Выполняет вызов API для создания заметки и запуск механизма синхронизации
        /// </summary>
        /// <param name="viewModel">Данные заметки</param>
        /// <returns></returns>
        private async Task AddNote(TextNoteViewModel viewModel)
        {
            var isAdded = await apiClient.Notes.AddNote(viewModel.Model);

            MessageQueue.Enqueue(isAdded ? "Заметка добавлена!" : "Возникла ошибка!");

            await Sync();
        }

        /// <summary>
        /// Выполняет смену темы интерфейса
        /// </summary>
        private void ChangeTheme()
        {
            if (Theme == ColorZoneMode.Light)
            {
                Theme = ColorZoneMode.Dark;
            }
            else
            {
                Theme = ColorZoneMode.Light;
            }
        }

        /// <summary>
        /// Выход из аккаунта и сброса токена
        /// </summary>
        private void Exit()
        {
            tokenSource.Cancel();
            apiClient.LogOut();

            NavigationManager.Current.GoTo("SignInPage");
        }

        /// <summary>
        /// Выполняет загрузку и синхронизацию заметок
        /// </summary>
        /// <returns></returns>
        private async Task Sync(NoteState selector = NoteState.Active)
        {
            logger.Info("Запуск синхронизации с сервером...");

            SyncAnimation = true;

            await Task.Delay(1500);

            var watch = Stopwatch.StartNew();
            var count = await apiClient.Notes.GetCount();

            if (count > 0)
            {
                if(selector != NoteState.Active)
                    Items.Clear();

                CenterLabelVisibility = Visibility.Hidden;

                var notes = (await LoadNotes()).Where(n => n.State == selector).ToArray();

                // Добавляем заметки
                foreach (var note in notes)
                {
                    note.RemoveEvent += RemoveNote;
                    note.ArchiveEvent += ArchiveNote;
                    note.EditEvent += EditNote;

                    if (!Items.Contains(note))
                    {
                        Items.Add(note);
                    }
                }

                // Удаляем те что не нужны
                for (int i = 0; i < Items.Count; i++)
                {
                    if (!notes.Contains(Items[i]))
                    {
                        Items.RemoveAt(i);
                    }
                }
            }
            else
            {
                CenterLabelVisibility = Visibility.Visible;
            }

            SyncAnimation = false;

            watch.Stop();
            logger.Info($"Синхронизация завершена. Выполнено за {watch.ElapsedMilliseconds} мс");
        }

        /// <summary>
        /// Загружает заметки и преобразует их в модели отображения <see cref="TextNoteViewModel"/>
        /// </summary>
        /// <returns>Массив <see cref="TextNoteViewModel"/></returns>
        private async Task<TextNoteViewModel[]> LoadNotes()
        {
            var notes = await apiClient.Notes.GetNotes();
            var viewModels = new List<TextNoteViewModel>(notes.Length);

            foreach (var note in notes)
            {
                viewModels.Add(new TextNoteViewModel(note));
            }

            return viewModels.ToArray();
        }

        /// <summary>
        /// Удаляет заметку и запускает синхронизацию
        /// </summary>
        /// <param name="viewModel">Модель отображения</param>
        private void RemoveNote(TextNoteViewModel viewModel)
        {
            Items.Remove(viewModel);

            apiClient.Notes.RemoveNote(viewModel.Id);

#pragma warning disable CS4014
            Sync();
#pragma warning restore CS4014
        }

        /// <summary>
        /// Редактирует заметку
        /// </summary>
        /// <param name="viewModel">Модель отображения</param>
        private async void EditNote(TextNoteViewModel viewModel)
        {
            var dialog = new AddEditTextNoteDialog(viewModel);
            var result = await OpenDialog(dialog);

            if (result)
            {
                dialog.UpdateViewModel(viewModel);

                await apiClient.Notes.UpdateNote(viewModel.Model);
                await Sync();
            }
        }

        /// <summary>
        /// Архивирует заметку
        /// </summary>
        /// <param name="viewModel">Модель отображения</param>
        private async void ArchiveNote(TextNoteViewModel viewModel)
        {
            viewModel.State = NoteState.Archived;

            await apiClient.Notes.UpdateNote(viewModel.Model);
            await Sync();
        }

        /// <summary>
        /// Проверяет, произошла ли рассинхронизация данных с сервером
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsOutOfSync()
        {
            var notes = await apiClient.Notes.GetNotes();

            if (notes.Length != Items.Count)
                return true;

            foreach (var item in Items)
            {
                if (notes.Any(n => n.Equals(item.Model)) == false)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Фоновая задача проверки синхронизации клиента с сервером
        /// </summary>
        /// <param name="interval">Интервал запуска</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        private async Task RunBackgroundTask(TimeSpan interval, CancellationToken cancellationToken)
        {
            await Task.Delay(interval, cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                SyncAnimation = true;

                await Task.Delay(TimeSpan.FromSeconds(1.5), cancellationToken);

                if (await IsOutOfSync())
                {
                    MessageQueue.Enqueue("Синхронизация...");

                    logger.Error("Ошибка синхронизации!");

                    await Sync();
                }
                else
                {
                    SyncAnimation = false;
                }

                await Task.Delay(interval, cancellationToken);
            }
        }

        /// <summary>
        /// Открывает диалог, блокируя операцию открытия <see cref="DialogHost"/> для остальных
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task<bool> OpenDialog(object content)
        {
            if (IsDialogOpened == false)
            {
                IsDialogOpened = true;

                var result = (bool)await DialogHost.Show(content);

                IsDialogOpened = false;

                return result;
            }

            return false;
        }

        #endregion
    }
}
