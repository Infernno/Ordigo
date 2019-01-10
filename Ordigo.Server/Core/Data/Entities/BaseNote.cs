using System;
using Newtonsoft.Json;
using Ordigo.Server.Core.Contracts;

namespace Ordigo.Server.Core.Data.Entities
{
    /// <summary>
    /// Базовая сущность, описывающая основные элементы заметки (ID, заголовок, состояние)
    /// </summary>
    public abstract class BaseNote : IEntityBase
    {
        /// <inheritdoc />
        /// <summary>
        /// ID заметки
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Владелец заметки
        /// </summary>
        [JsonIgnore]
        public Account Owner { get; set; }

        /// <summary>
        /// Заголовок заметки
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Дата создания заметки
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;

        /// <summary>
        /// Статус заметки
        /// </summary>
        public NoteState State { get; set; } = NoteState.Active;

        /// <summary>
        /// Фоновый цвет заметки в формате HEX
        /// </summary>
        public string BackgroundColor { get; set; }
    }
}
