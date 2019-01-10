using System.ComponentModel.DataAnnotations;

namespace Ordigo.Server.Core.Requests
{
    /// <summary>
    /// DTO, необходимый для передачи данных для регистрации
    /// </summary>
    public class SignUpRequest
    {
        /// <summary>
        /// Логин (имя пользователя)
        /// </summary>
        [Required(ErrorMessage = "Логин не может быть пустым")]
#if RELEASE
        [MinLength(RequestRequirements.UsernameMinLength, ErrorMessage = "Длина логина не может быть меньше 3 символов")]
#endif
        public string Username { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required(ErrorMessage = "Пароль не может быть пустым")]
#if RELEASE
        [MinLength(RequestRequirements.PasswordMinLength, ErrorMessage = "Длина пароля не может быть меньше 3 символов")]
#endif
        public string Password { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [Required(ErrorMessage = "Адрес электронный почты не может быть пустым")]
#if RELEASE
        [RegularExpression(RequestRequirements.EmailRegex, ErrorMessage = "Некорректный адрес электронной почты")]
#endif
        public string Email { get; set; }
    }
}
