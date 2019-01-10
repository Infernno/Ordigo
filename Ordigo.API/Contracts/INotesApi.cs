using System.Threading.Tasks;
using Ordigo.API.Model;

namespace Ordigo.API.Contracts
{
    /// <summary>
    /// Интерфейс для работы с API заметок (создание, удаление и т.д.)
    /// </summary>
    public interface INotesApi
    {
        /// <summary>
        /// Возвращает все заметки пользователя
        /// </summary>
        /// <returns></returns>
        Task<TextNoteModel[]> GetNotes();

        /// <summary>
        /// Возвращает общее количество заметок пользователя
        /// </summary>
        /// <returns></returns>
        Task<int> GetCount();

        /// <summary>
        /// Создает текстовую заметку
        /// </summary>
        /// <param name="note">Данные для создания заметки</param>
        /// <returns></returns>
        Task<bool> AddNote(TextNoteModel note);

        /// <summary>
        /// Обновляет данные заметки
        /// </summary>
        /// <param name="note">Данные заметки</param>
        /// <returns></returns>
        Task<bool> UpdateNote(TextNoteModel note);

        /// <summary>
        /// Удаляет текстовую заметку
        /// </summary>
        /// <param name="id">ID заметки</param>
        /// <returns></returns>
        Task<bool> RemoveNote(int id);
    }
}
