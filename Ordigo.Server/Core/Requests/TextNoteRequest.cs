using System.ComponentModel.DataAnnotations;
using Ordigo.Server.Core.Data.Entities;

namespace Ordigo.Server.Core.Requests
{
    public class TextNoteRequest
    {
        /// <summary>
        /// ID заметки
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Заголовок заметки
        /// </summary>
        [Required(ErrorMessage = "Заголовок не может быть пустым")]
        [MinLength(RequestRequirements.NoteTitleMin, ErrorMessage = "Заголовок заметки не может быть меньше 3 символов")]
        public string Title { get; set; }

        /// <summary>
        /// Содержимое заметки
        /// </summary>
        [Required(ErrorMessage = "Содержимое заметки не может быть пустым")]
        [MinLength(RequestRequirements.NoteContentMin, ErrorMessage = "Содержимое заметки не может быть меньше 3 символов")]
        public string Content { get; set; }

        /// <summary>
        /// Фоновый цвет заметки
        /// </summary>
        [RegularExpression(RequestRequirements.NoteColorRegex, ErrorMessage = "Неверная кодировка цвета")]
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Состояние заметки
        /// </summary>
        public NoteState State { get; set; }
    }
}
