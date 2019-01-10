using System.Threading.Tasks;

namespace Ordigo.API.Contracts
{
    /// <summary>
    /// Интерфейс авторизации через API
    /// </summary>
    public interface IApiAuthAsync
    {
        /// <summary>
        /// Авторизован ли пользователь
        /// </summary>
        bool IsAuthorized { get; }

        /// <summary>
        /// Выполняет вход в учетную запись
        /// </summary>
        /// <param name="username">Имя пользователя для входа</param>
        /// <param name="password">Пароль для входа</param>
        Task SignIn(string username, string password);

        /// <summary>
        /// Выполняет создание (регистрацию) новой учетные записи
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="email">Адрес электронной почты</param>
        /// <param name="password">Пароль</param>
        Task SignUp(string username, string email, string password);

        /// <summary>
        /// Выход из учетной записи
        /// </summary>
        void LogOut();
    }
}
