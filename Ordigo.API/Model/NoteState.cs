namespace Ordigo.API.Model
{
    /// <summary>
    /// Статус заметки
    /// </summary>
    public enum NoteState
    {
        /// <summary>
        /// Заметка активна
        /// </summary>
        Active = 0,

        /// <summary>
        /// Заметка архивирована
        /// </summary>
        Archived = 1,

        /// <summary>
        /// Заметка в корзине
        /// </summary>
        Trash = 2
    }
}
