using System;
using System.Collections.Generic;

namespace Ordigo.API.Infrastructure
{
    /// <inheritdoc />
    /// <summary>
    /// Класс, описывающий исключение при вызове API функций
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int ErrorCode { get; }

        /// <summary>
        /// Ошибки валидации данных (если есть)
        /// </summary>
        public Dictionary<string, string> ValidationErrors { get; }

        /// <summary>
        /// Указывает, возникли ли ошибки валидации данных при запросе
        /// </summary>
        public bool HasValidationErrors => ValidationErrors != null && ValidationErrors.Count > 0;

        /// <inheritdoc />
        /// <summary>
        /// Создает исключение об ошибке API запроса
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="errorCode">Код ошибки</param>
        public ApiException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <inheritdoc />
        /// <summary>
        /// Создает исключение об ошибке API запроса
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="errorCode">Код ошибки</param>
        /// <param name="validationErrors">Ошибки валидации данных</param>
        public ApiException(string message, int errorCode, Dictionary<string, string> validationErrors) : base(message)
        {
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }
    }
}
