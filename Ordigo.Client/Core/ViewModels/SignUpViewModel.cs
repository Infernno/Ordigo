using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Ordigo.API.Contracts;
using Ordigo.API.Infrastructure;
using Ordigo.API.Model;
using Ordigo.Client.Core.UI;
using PropertyChanged;

namespace Ordigo.Client.Core.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// Модель отображения для экрана регистрации
    /// </summary>
    public class SignUpViewModel : INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// Создает экземпляр <see cref="SignUpViewModel"/>
        /// </summary>
        /// <param name="client">Класс, реализующий работу с API</param>
        /// <param name="messageQueue">Очередь сообщений</param>
        public SignUpViewModel(IApiClient client, ISnackbarMessageQueue messageQueue)
        {
            apiClient = client;
            MessageQueue = messageQueue;

            SignUpCommand = new RelayCommand(SignUp, CanSignUp);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Событие, оповещающее об измении какого-либо свойства в модели
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// API для работы с авторизацией
        /// </summary>
        private readonly IApiClient apiClient;

        #endregion

        #region Properties

        /// <summary>
        /// Имя пользователя на экране регистрации
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Адрес электронной почты на экране входа
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя на экране регистрации
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Указывает, происходит ли авторизация в данные момент
        /// </summary>
        public bool InProcess { get; set; }

        /// <summary>
        /// Команда обработки для кнопки регистрации
        /// </summary>
        [DoNotNotify]
        public ICommand SignUpCommand { get; }

        /// <summary>
        /// Очередь сообщений для снэкбара
        /// </summary>
        [DoNotNotify]
        public ISnackbarMessageQueue MessageQueue { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Указывает, возможно ли выполнить команду регистрации с текущими введенными данными
        /// </summary>
        /// <returns>True - данные валидны и вход возможен, False - нет</returns>
        private bool CanSignUp()
        {
            return !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(Password);
        }

        /// <summary>
        /// Выполняет регистрацию и вход через API сервера
        /// </summary>
        private async void SignUp()
        {
            if (InProcess)
                return;

            InProcess = true;

            await Task.Delay(1500);

            try
            {
                // Создает учетную запись пользователя
                await apiClient.SignUp(Username, Email, Password);

                // Выполняем вход
                await apiClient.SignIn(Username, Password);

                // Если вход успешно выполнен
                if (apiClient.IsAuthorized)
                {
                    // Создает первую заметку
                    await MakeDemoNote();

                    // Переходим к главному экрану
                    NavigationManager.Current.GoTo("MainPage");
                }
            }
            catch (WebException)
            {
                MessageQueue.Enqueue("Сервер недоступен!");
            }
            catch (ApiException ex)
            {
                if (ex.HasValidationErrors)
                {
                    var error = ex.ValidationErrors.Values.First();

                    MessageQueue.Enqueue(error);
                }
                else
                {
                    MessageQueue.Enqueue(ex.Message);
                }
            }

            InProcess = false;
        }

        private async Task MakeDemoNote()
        {
            // Необязательный шаг, но все равно
            int count = await apiClient.Notes.GetCount();

            if (count <= 0)
            {
                var note = new TextNoteModel
                {
                    Title = "Привет мир!",
                    Content = "Это моя первая заметка.",
                    BackgroundColor = Color.Purple
                };

                await apiClient.Notes.AddNote(note);
            }
        }

        #endregion
    }
}
