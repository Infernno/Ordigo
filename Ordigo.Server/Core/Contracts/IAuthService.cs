using Ordigo.Server.Core.Data.Entities;

namespace Ordigo.Server.Core.Contracts
{
    /// <summary>
    /// Сервис авторизации и регистрации пользователей
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Проверяет, возможен ли вход с указанным логином и паролем
        /// </summary>
        /// <param name="username">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns><see cref="Account"/>, если вход успешен. Null если нет.</returns>
        Account SignIn(string username, string password);

        /// <summary>
        /// Выполняет регистрацию новой учетной записи пользователя в системе
        /// </summary>
        /// <param name="account">Данные для регистрации</param>
        /// <param name="password">Пароль для хеширования</param>
        /// <returns>True - если регистрация прошла успешно, False - ошибка (например такой пользователь уже есть в системе)</returns>
        bool SignUp(Account account, string password);
    }
}