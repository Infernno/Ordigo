using System;
using System.Drawing;
using Newtonsoft.Json;
using Ordigo.API.Serialization;

namespace Ordigo.API.Model
{
    public sealed class TextNoteModel
    {
        #region Properties

        /// <summary>
        /// ID заметки
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Заголовок заметки
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Содержимое заметки
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Дата создания заметки
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Статус заметки
        /// </summary>
        public NoteState State { get; set; }

        /// <summary>
        /// Фоновый цвет заметки
        /// </summary>
        [JsonConverter(typeof(JsonHexColorConverter))]
        public Color BackgroundColor { get; set; } = ColorTranslator.FromHtml("#03a9f4");

        #endregion

        #region Equals

        private bool Equals(TextNoteModel other)
        {
            return string.Equals(Title, other.Title) && 
                   string.Equals(Content, other.Content) && 
                   Created.Equals(other.Created) && 
                   State == other.State && 
                   BackgroundColor.Equals(other.BackgroundColor);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj is TextNoteModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Title != null ? Title.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Content != null ? Content.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Created.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) State;
                hashCode = (hashCode * 397) ^ BackgroundColor.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}
