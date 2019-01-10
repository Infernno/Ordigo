using Ordigo.API.Model;
using Ordigo.Client.Core.UI;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using PropertyChanged;

namespace Ordigo.Client.Core.ViewModels
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    /// Модель отображения текстовой заметки
    /// </summary>
    [DoNotNotify]
    public class TextNoteViewModel : INotifyPropertyChanged
    {
        #region Constructor

        public TextNoteViewModel(TextNoteModel model = null)
        {
            Model = model ?? new TextNoteModel();

            DeleteCommand = new RelayCommand(RemoveNote);
            ArchiveCommand = new RelayCommand(ArchiveNote);
            EditCommand = new RelayCommand(EditNote);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Событие, оповещающее об измении какого-либо свойства в модели
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Событие, оповещающее об необходимости редактирования модели
        /// </summary>
        public event Action<TextNoteViewModel> EditEvent;

        /// <summary>
        /// Событие, оповещающее об необходимости архивирования
        /// </summary>
        public event Action<TextNoteViewModel> ArchiveEvent;

        /// <summary>
        /// Событие, оповещающее об необходимости удаления
        /// </summary>
        public event Action<TextNoteViewModel> RemoveEvent;

        /// <summary>
        /// Кэш фонового цвета заметки
        /// </summary>
        private SolidColorBrush brushCache;

        #endregion

        #region Properties

        /// <summary>
        /// ID заметки
        /// </summary>
        public TextNoteModel Model { get; }

        /// <summary>
        /// ID заметки
        /// </summary>
        public int Id => Model.Id;

        /// <summary>
        /// Заголовок заметки
        /// </summary>
        public string Title
        {
            get => Model.Title;
            set
            {
                Model.Title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Содержимое заметки
        /// </summary>
        public string Content
        {
            get => Model.Content;
            set
            {
                Model.Content = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Текущее состояние заметки
        /// </summary>
        public NoteState State
        {
            get => Model.State;
            set
            {
                Model.State = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Дата создания заметки в форматированном виде
        /// </summary>
        public string СreatedFormat => Model.Created.ToShortDateString();

        /// <summary>
        /// Фоновый цвет заметки
        /// </summary>
        public SolidColorBrush BackgroundColorBrush
        {
            get
            {
                if (brushCache == null)
                {
                    var color = Model.BackgroundColor;

                    byte alpha = color.A;
                    byte red = color.R;
                    byte green = color.G;
                    byte blue = color.B;

                    brushCache = new SolidColorBrush(Color.FromArgb(alpha, red, green, blue));
                }

                return brushCache;
            }
        }

        /// <summary>
        /// Команда для удаления заметки
        /// </summary>
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Команда для архивирования заметки
        /// </summary>
        public ICommand ArchiveCommand { get; }

        /// <summary>
        /// Команда для редактирования заметки
        /// </summary>
        public ICommand EditCommand { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Вызывает событие удаления заметки
        /// </summary>
        private void RemoveNote()
        {
            // Вызываем событие 
            RemoveEvent?.Invoke(this);
        }

        /// <summary>
        /// Вызывает событие архивирования заметки
        /// </summary>
        private void ArchiveNote()
        {
            ArchiveEvent?.Invoke(this);
        }

        /// <summary>
        /// Вызывает событие редактирования заметки 
        /// </summary>
        private void EditNote()
        {
            EditEvent?.Invoke(this);
        }

        /// <summary>
        /// Вызывает событие PropertyChanged
        /// </summary>
        /// <param name="propertyName">Имя свойства для которого вызывается событие</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Equals

        protected bool Equals(TextNoteViewModel other)
        {
            return Equals(Model, other.Model);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((TextNoteViewModel)obj);
        }

        public override int GetHashCode()
        {
            return Model != null ? Model.GetHashCode() : 0;
        }

        #endregion

    }
}
