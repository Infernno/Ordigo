using Ordigo.Server.Core.Data.Entities;

namespace Ordigo.Server.Core.Contracts
{
    /// <summary>
    /// Сервис для работы с заметками
    /// </summary>
    public interface INoteService
    {
        /// <summary>
        /// Возвращает массив текстовых заметок 
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        TextNote[] GetTextNotes(int userId);

        /// <summary>
        /// Создает текстовую заметку
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="textNote">Данные заметки</param>
        void AddTextNote(int userId, TextNote textNote);

        /// <summary>
        /// Обновляет данные текстовой заметки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="textNote">Данные заметки</param>
        void UpdateTextNote(int userId, TextNote textNote);

        /// <summary>
        /// Удаляет текстовую заметку для указанного пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="noteId">Идентификатор заметки</param>
        void RemoveTextNote(int userId, int noteId);
    }
}