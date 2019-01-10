using System;
using System.Linq;
using Ordigo.Server.Core.Contracts;
using Ordigo.Server.Core.Data.Entities;

namespace Ordigo.Server.Core.Services
{
    /// <inheritdoc />
    /// <summary>
    /// Сервис для работы с заметками
    /// </summary>
    public class NoteService : INoteService
    {
        #region Constructor

        public NoteService(ITextNoteRepository textNoteRepository, IAccountRepository accountRepository)
        {
            this.textNoteRepository = textNoteRepository;
            this.accountRepository = accountRepository;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Репозиторий для работы с текстовыми заметками из БД
        /// </summary>
        private readonly ITextNoteRepository textNoteRepository;

        /// <summary>
        /// Репозиторий для работы с учетными записями из БД
        /// </summary>
        private readonly IAccountRepository accountRepository;

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        /// Возвращает массив текстовых заметок 
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        public TextNote[] GetTextNotes(int userId)
        {
            return textNoteRepository.GetAll()
                .Where(n => n.Owner.Id == userId)
                .ToArray();
        }

        /// <inheritdoc />
        /// <summary>
        /// Создает текстовую заметку
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="textNote">Данные заметки</param>
        public void AddTextNote(int userId, TextNote textNote)
        {
            var account = accountRepository.GetById(userId);

            if (account != null)
            {
                textNote.Owner = account;

                textNoteRepository.Add(textNote);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Обновляет данные текстовой заметки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="textNote">Данные заметки</param>
        public void UpdateTextNote(int userId, TextNote textNote)
        {
            var note = GetNoteForUser(userId, textNote.Id);

            note.Title = textNote.Title;
            note.Content = textNote.Content;
            note.BackgroundColor = textNote.BackgroundColor;
            note.State = textNote.State;

            textNoteRepository.Update(note);
        }

        /// <summary>
        /// Удаляет текстовую заметку для указанного пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="noteId">Идентификатор заметки</param>
        public void RemoveTextNote(int userId, int noteId)
        {
            var note = GetNoteForUser(userId, noteId);

            textNoteRepository.Remove(note);
        }

        /// <summary>
        /// Выполняет поиск текстовой заметки в базе данных
        /// Проверяет чтобы у указанного пользователя была заметка с указанным ID
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="noteId">Идентификатор заметки</param>
        /// <returns></returns>
        private TextNote GetNoteForUser(int userId, int noteId)
        {
            var note = textNoteRepository.Find(n => n.Id == noteId && n.Owner.Id == userId);

            if (note == null)
                throw new InvalidOperationException($"Note #{noteId} for account {userId} doesn't exist!");

            return note;
        }

        #endregion
    }
}
