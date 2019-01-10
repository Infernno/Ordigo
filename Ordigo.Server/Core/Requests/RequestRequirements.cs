// ReSharper disable UnusedMember.Global

namespace Ordigo.Server.Core.Requests
{
    /// <summary>
    /// Класс, содержащий требования к входных данным запроса
    /// <para>Поскольку данные в любых атрибутах необходимо знать на этапе компиляции, то здесь они указаны как константы</para>
    /// </summary>
    internal static class RequestRequirements
    {
        /// <summary>
        /// Минимальная длина имени пользователя
        /// </summary>
        public const int UsernameMinLength = 3;

        /// <summary>
        /// Минимальная длина пароля
        /// </summary>
        public const int PasswordMinLength = 3;

        /// <summary>
        /// Регулярное выражения для проверки адреса электронной почты
        /// </summary>
        public const string EmailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        /// <summary>
        /// Минимальная длина заголовка заметки
        /// </summary>
        public const int NoteTitleMin = 3;

        /// <summary>
        /// Минимальная длина содержимого заметки
        /// </summary>
        public const int NoteContentMin = 3;

        /// <summary>
        /// Регулярное выражения для проверки HEX цвета
        /// </summary>
        public const string NoteColorRegex = @"^#(?:[0-9a-fA-F]{3}){1,2}$";
    }
}
