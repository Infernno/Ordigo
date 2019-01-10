using Ordigo.Server.Core.Contracts;

namespace Ordigo.Server.Core.Data.Entities
{
    /// <summary>
    /// Сущность, описывающая учетную запись пользователя
    /// </summary>
    public class Account : IEntityBase
    {
        /// <summary>
        /// ID аккаунта
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Хеш пароля
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }
    }
}
