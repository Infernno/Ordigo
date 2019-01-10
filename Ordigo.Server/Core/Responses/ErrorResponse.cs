using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Ordigo.Server.Core.Validation;

namespace Ordigo.Server.Core.Responses
{
    /// <summary>
    /// Код ошибки
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// Ошибка проверки входных данных
        /// </summary>
        ValidationError = 0,

        /// <summary>
        /// Неверные данные для входа (логин / пароль)
        /// </summary>
        InvalidCredentials = 1,

        /// <summary>
        /// Такой аккаунт уже существует
        /// </summary>
        AccountExists = 2,

        /// <summary>
        /// Ошибка при удалении заметки
        /// </summary>
        NoteRemovalError = 3
    }

    /// <summary>
    /// Класс, содержащий сообщение об ошибке
    /// </summary>
    public class ErrorData
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        [JsonProperty("error_code")]
        public int Code { get; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        [JsonProperty("error_msg")]
        public string Message { get; }

        /// <summary>
        /// Ошибки валидации
        /// </summary>
        [JsonProperty("validation_errors", NullValueHandling = NullValueHandling.Ignore)]
        public ValidationResultModel ValidationResult { get; }

        public ErrorData(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public ErrorData(int code, string message, ValidationResultModel validationResult)
        {
            Code = code;
            Message = message;
            ValidationResult = validationResult;
        }
    }

    public class ErrorResponse
    {
        #region Fields

        public static readonly ErrorResponse InvalidCredentials = new ErrorResponse(ErrorCode.InvalidCredentials, "Неверный логин или пароль");
        public static readonly ErrorResponse AccountExists = new ErrorResponse(ErrorCode.AccountExists, "Такой пользователь уже существует");

        #endregion

        #region Properties

        /// <summary>
        /// Данные об ошибке
        /// </summary>
        [JsonProperty("error")]
        public ErrorData Error { get; }

        #endregion

        #region Constructor

        public ErrorResponse(ErrorData error)
        {
            Error = error;
        }

        public ErrorResponse(int code, string message) : this(new ErrorData(code, message))
        {

        }

        public ErrorResponse(ErrorCode code, string message) : this(new ErrorData((int)code, message))
        {

        }

        public ErrorResponse(int code, string message, ValidationResultModel validationResult)
            : this(new ErrorData(code, message, validationResult))
        {

        }

        public ErrorResponse(ErrorCode code, string message, ValidationResultModel validationResult)
            : this(new ErrorData((int)code, message, validationResult))
        {

        }

        #endregion

        #region Methods

        public static ErrorResponse ValidationError(ModelStateDictionary model)
        {
            return new ErrorResponse(ErrorCode.ValidationError, "Ошибка валидации данных", new ValidationResultModel(model));
        }

        #endregion
    }
}
