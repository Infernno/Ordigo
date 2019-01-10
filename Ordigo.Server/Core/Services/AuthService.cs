using Microsoft.AspNetCore.Identity;
using Ordigo.Server.Core.Contracts;
using Ordigo.Server.Core.Data.Entities;

namespace Ordigo.Server.Core.Services
{
    /// <inheritdoc />
    /// <summary>
    /// Сервис авторизации и регистрации пользователей
    /// </summary>
    public class AuthService : IAuthService
    {
        #region Constructor

        public AuthService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
            this.passwordHasher = new PasswordHasher<Account>();
        }

        #endregion

        #region Fields

        /// <summary>
        /// Репозиторий для работы с базой данных
        /// </summary>
        private readonly IAccountRepository accountRepository;

        /// <summary>
        /// Хешер паролей
        /// </summary>
        private readonly PasswordHasher<Account> passwordHasher;

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        /// Проверяет, возможен ли вход с указанным логином и паролем
        /// </summary>
        /// <param name="username">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns><see cref="Account" />, если вход успешен. Null если нет.</returns>
        public Account SignIn(string username, string password)
        {
            var user = accountRepository.Find(a => a.Username == username);

            if (user != null)
            {
                var result = passwordHasher.VerifyHashedPassword(null, user.PasswordHash, password);

                if (result != PasswordVerificationResult.Failed)
                {
                    return user;
                }
            }

            return null;
        }

        /// <inheritdoc />
        /// <summary>
        /// Выполняет регистрацию новой учетной записи пользователя в системе
        /// </summary>
        /// <param name="account">Данные для регистрации</param>
        /// <param name="password">Пароль для хеширования</param>
        /// <returns>True - если регистрация прошла успешно, False - ошибка (например такой пользователь уже есть в системе)</returns>
        public bool SignUp(Account account, string password)
        {
            if (accountRepository.Contains(a => a.Username == account.Username || a.Email == account.Email))
            {
                return false;
            }

            try
            {
                account.PasswordHash = passwordHasher.HashPassword(null, password);
                accountRepository.Add(account);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion

    }
}
