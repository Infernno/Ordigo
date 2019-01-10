using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordigo.Server.Core.Contracts;
using Ordigo.Server.Core.Data.Entities;
using Ordigo.Server.Core.Responses;
using System.Security.Claims;
using Ordigo.Server.Core.Requests;

namespace Ordigo.Server.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Контроллер, отвечающий за авторизацию и регистрацию пользователей
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : Controller
    {
        #region Constructor

        public AuthController(IAuthService authService, IAuthTokenFactory tokenFactory, IMapper mapper)
        {
            this.authService = authService;
            this.tokenFactory = tokenFactory;
            this.mapper = mapper;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Сервис, отвечающий за авторизацию и регистрацию пользователей
        /// </summary>
        private readonly IAuthService authService;

        /// <summary>
        /// Фабрика, генерирующая JWT токен авторизации
        /// </summary>
        private readonly IAuthTokenFactory tokenFactory;
        
        /// <summary>
        /// Маппер объектов (преобразует из одного типа в другой)
        /// </summary>
        private readonly IMapper mapper;

        #endregion

        #region Methods

        /// <summary>
        /// Выполняет авторизацию пользователя
        /// </summary>
        /// <param name="request">Данные для авторизации в виде <see cref="SignInRequest"/></param>
        /// <returns><see cref="SignInResponse"/> в случае удачи или <see cref="ErrorResponse"/> при ошибке</returns>
        [HttpGet]
        [Route("sign.in")]
        public IActionResult SignIn([FromQuery] SignInRequest request)
        {
            var account = authService.SignIn(request.Username, request.Password);

            if (account != null)
            {
                var token = CreateToken(account);
                var response = new SignInResponse(account.Username, token);

                return Json(response);
            }

            return Json(ErrorResponse.InvalidCredentials);
        }

        /// <summary>
        /// Выполняет регистрацию пользователя
        /// </summary>
        /// <param name="request">Данные для регистрации в виде <see cref="SignUpRequest"/></param>
        /// <returns><see cref="SignUpRequest"/> в случае удачи или <see cref="ErrorResponse"/> при ошибке</returns>
        [HttpGet]
        [Route("sign.up")]
        public IActionResult SignUp([FromQuery] SignUpRequest request)
        {
            var account = mapper.Map<Account>(request);

            if (authService.SignUp(account, request.Password))
            {
                var response = new SignUpResponse(true, request.Username);

                return Json(response);
            }

            return Json(ErrorResponse.AccountExists);
        }

        /// <summary>
        /// Создает токен авторизации
        /// </summary>
        /// <param name="account">Пользователь, для которого создается токен</param>
        /// <returns>JWT токен в виде строки</returns>
        private string CreateToken(Account account)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, account.Username),    // Имя пользователя
                new Claim(ClaimTypes.Sid, account.Id.ToString()) // ID пользователя
            };

            var token = tokenFactory.Generate(claims);

            return token;
        }

        #endregion
    }
}
