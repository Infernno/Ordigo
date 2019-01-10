using MaterialDesignThemes.Wpf;
using Ordigo.Client.Core.UI;
using PropertyChanged;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Ordigo.API.Contracts;
using Ordigo.API.Infrastructure;

namespace Ordigo.Client.Core.ViewModels
{
    /// <summary>
    /// Модель отображения для экрана входа
    /// </summary>
    public class SignInViewModel : INotifyPropertyChanged
    {
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
        /// Имя пользователя на экране входа
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль пользователя на экране входа
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Указывает, происходит ли авторизация в данные момент
        /// </summary>
        public bool InProcess { get; set; }

        /// <summary>
        /// Команда обработки для кнопки входа
        /// </summary>
        [DoNotNotify]
        public ICommand SignInCommand { get; }

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

        #region Constructor

        /// <summary>
        /// Создает экземпляр <see cref="SignInViewModel"/>
        /// </summary>
        /// <param name="client">Класс, реализующий работу с API</param>
        /// <param name="messageQueue">Очередь сообщений</param>
        public SignInViewModel(IApiClient client, ISnackbarMessageQueue messageQueue)
        {
            apiClient = client;
            MessageQueue = messageQueue;

            SignInCommand = new RelayCommand(SignIn, CanSignIn);
            SignUpCommand = new RelayCommand(SignUp);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Указывает, возможно ли выполнить команду входа с текущими введенными данными
        /// </summary>
        /// <returns>True - данные валидны и вход возможен, False - нет</returns>
        private bool CanSignIn()
        {
            return !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Password);
        }

        /// <summary>
        /// Выполняет авторизацию через API сервера
        /// </summary>
        private async void SignIn()
        {
            if (InProcess)
                return;

            InProcess = true;

            await Task.Delay(1500);

            try
            {
                await apiClient.SignIn(Username, Password);

                if (apiClient.IsAuthorized)
                {
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

        /// <summary>
        /// Отправляет пользователя к экрану регистрации
        /// </summary>
        private void SignUp()
        {
            NavigationManager.Current.GoTo("SignUpPage");
        }

        #endregion
    }
}