namespace Ordigo.Server.Core.Data.Entities
{
    /// <summary>
    /// Сущность, описывающая обычную текстовую заметку
    /// </summary>
    public class TextNote : BaseNote
    {
        /// <summary>
        /// Содержимое заметки
        /// </summary>
        public string Content { get; set; }
    }
}