using System;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordigo.Server.Core.Contracts;
using Ordigo.Server.Core.Data.Entities;
using Ordigo.Server.Core.Requests;
using Ordigo.Server.Core.Responses;

namespace Ordigo.Server.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Контроллер, отвечающий за работу с заметками
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api")]
    public class NotesController : Controller
    {
        #region Constructor

        public NotesController(INoteService noteService, IMapper mapper)
        {
            this.noteService = noteService;
            this.mapper = mapper;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Сервис для работы с заметками
        /// </summary>
        private readonly INoteService noteService;

        /// <summary>
        /// Маппер объектов (преобразует из одного типа в другой)
        /// </summary>
        private readonly IMapper mapper;

        #endregion

        #region Methods

        /// <summary>
        /// Возвращает все заметки пользователя
        /// </summary>
        [Route("notes.all")]
        public IActionResult GetAllNotes()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Sid).Value);
            var notes = noteService.GetTextNotes(userId);

            return Json(notes);
        }

        /// <summary>
        /// Возвращает количество заметок пользователя
        /// </summary>
        [Route("notes.count")]
        public IActionResult GetNotesCount()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Sid).Value);
            var notes = noteService.GetTextNotes(userId);

            return Json(new { note_count = notes.Length });
        }

        /// <summary>
        /// Создает текстовую заметку пользователя
        /// </summary>
        /// <param name="request">Данные для создания заметки</param>
        [HttpGet]
        [Route("notes.text.add")]
        public IActionResult AddTextNote([FromQuery] TextNoteRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Sid).Value);
            var note = mapper.Map<TextNote>(request);

            try
            {
                noteService.AddTextNote(userId, note);
            }
            catch (Exception ex)
            {
                return Json(new ErrorResponse(-1, ex.Message));
            }

            return Json(new { successful = true });
        }

        /// <summary>
        /// Создает текстовую заметку пользователя
        /// </summary>
        /// <param name="request">Данные для создания заметки</param>
        [HttpGet]
        [Route("notes.text.update")]
        public IActionResult UpdateTextNote([FromQuery] TextNoteRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Sid).Value);
            var note = mapper.Map<TextNote>(request);

            try
            {
                noteService.UpdateTextNote(userId, note);
            }
            catch (Exception ex)
            {
                return Json(new ErrorResponse(ex.HResult, ex.Message));
            }

            return Json(new { successful = true });
        }

        /// <summary>
        /// Удаляет текстовую заметку
        /// </summary>
        /// <param name="id">ID заметки для удаления</param>
        /// <returns></returns>
        [HttpGet]
        [Route("notes.text.remove")]
        public IActionResult RemoveTextNote([FromQuery] int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Sid).Value);

            try
            {
                noteService.RemoveTextNote(userId, id);
            }
            catch (Exception ex)
            {
                return Json(new ErrorResponse(-1, ex.Message));
            }

            return Json(new { successful = true });
        }

        #endregion
    }
}
