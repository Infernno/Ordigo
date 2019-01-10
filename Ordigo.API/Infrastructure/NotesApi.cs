using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ordigo.API.Contracts;
using Ordigo.API.Extensions;
using Ordigo.API.Model;

namespace Ordigo.API.Infrastructure
{
    /// <inheritdoc />
    /// <summary>
    /// Интерфейс для работы с API заметок
    /// </summary>
    public class NotesApi : INotesApi
    {
        #region Constructor

        public NotesApi(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Название API метода, который возвращает все заметки
        /// </summary>
        private const string AllNotesMethod = "notes.all";

        /// <summary>
        /// Название API метода, который возвращает общее количество заметок пользователя
        /// </summary>
        private const string NoteCountMethod = "notes.count";

        /// <summary>
        /// Название API метода, который создает текстовую заметку
        /// </summary>
        private const string AddTextNoteMethod = "notes.text.add";

        /// <summary>
        /// Название API метода, который обновляет текстовую заметку
        /// </summary>
        private const string UpdateTextNoteMethod = "notes.text.update";

        /// <summary>
        /// Название API метода, который удаляет текстовую заметку
        /// </summary>
        private const string RemoveTextNoteMethod = "notes.text.remove";

        /// <summary>
        /// Клиент для взаимодействия с API сервера
        /// </summary>
        private readonly IApiClient apiClient;

        #endregion

        #region Methods

        /// <summary>
        /// Возвращает все заметки пользователя
        /// </summary>
        /// <returns></returns>
        public async Task<TextNoteModel[]> GetNotes()
        {
            var response = await apiClient.CallAsync(AllNotesMethod, null);
            var notebook = JsonConvert.DeserializeObject<TextNoteModel[]>(response.RawJson);

            return notebook;
        }

        /// <summary>
        /// Возвращает общее количество заметок пользователя
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetCount()
        {
            var response = await apiClient.CallAsync(NoteCountMethod, null);
            var count = (int)response["note_count"];

            return count;
        }

        /// <summary>
        /// Создает текстовую заметку
        /// </summary>
        /// <param name="note">Данные для создания заметки</param>
        /// <returns></returns>
        public async Task<bool> AddNote(TextNoteModel note)
        {
            var parameters = new Dictionary<string, string>
            {
                { "title", note.Title },
                { "content", note.Content },
                { "backgroundcolor", note.BackgroundColor.ToHex() }
            };

            var response = await apiClient.CallAsync(AddTextNoteMethod, parameters);

            return response["successful"];
        }

        /// <summary>
        /// Обновляет данные заметки
        /// </summary>
        /// <param name="note">Данные заметки</param>
        /// <returns></returns>
        public async Task<bool> UpdateNote(TextNoteModel note)
        {
            var parameters = new Dictionary<string, string>
            {
                { "id", note.Id.ToString() },
                { "title", note.Title },
                { "content", note.Content },
                { "backgroundcolor", note.BackgroundColor.ToHex() },
                { "state", note.State.ToString() }
            };

            var response = await apiClient.CallAsync(UpdateTextNoteMethod, parameters);

            return response["successful"];
        }

        /// <summary>
        /// Удаляет текстовую заметку
        /// </summary>
        /// <param name="id">ID заметки</param>
        /// <returns></returns>
        public async Task<bool> RemoveNote(int id)
        {
            var parameters = new Dictionary<string, string>
            {
                { "id", id.ToString() }
            };

            var response = await apiClient.CallAsync(RemoveTextNoteMethod, parameters);

            return response["successful"];
        }

        #endregion
    }
}
