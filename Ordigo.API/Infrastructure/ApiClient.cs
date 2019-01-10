using NLog;
using Ordigo.API.Contracts;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ordigo.API.Infrastructure
{
    /// <inheritdoc />
    /// <summary>
    /// Класс, реализующий работу с API сервера
    /// </summary>
    public class ApiClient : IApiClient
    {
        #region Constructor

        /// <summary>
        /// Конструктор класса <see cref="ApiClient" />
        /// </summary>
        /// <param name="restClient">REST клиент для выполнения запросов</param>
        public ApiClient(IRestClient restClient)
        {
            this.restClient = restClient;

            Notes = new NotesApi(this);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Имя метода для авторизации
        /// </summary>
        private const string SignInMethod = "sign.in";

        /// <summary>
        /// Имя метода для регистрации
        /// </summary>
        private const string SignUpMethod = "sign.up";

        /// <summary>
        /// REST клиент для запросов
        /// </summary>
        private readonly IRestClient restClient;

        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Properties

        /// <inheritdoc />
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Текущий токен доступа для запросов
        /// </summary>
        public string AccessToken { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Проверяет авторизован ли пользователь
        /// </summary>
        public bool IsAuthorized
        {
            get
            {
                if (string.IsNullOrEmpty(AccessToken))
                    return false;

                return true;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// API для работы с заметками
        /// </summary>
        public INotesApi Notes { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        /// Асинхронный вызов API метода сервера
        /// </summary>
        /// <param name="method">Название метода</param>
        /// <param name="parameters">Параметры вызова метода в виде словаря</param>
        /// <returns>Ответ в виде <see cref="ApiResponse" /></returns>
        public async Task<ApiResponse> CallAsync(string method, IDictionary<string, string> parameters)
        {
            var request = BuildRequest(method, parameters);
            var response = await restClient.ExecuteTaskAsync(request);

            logger.Info($"Результат вызова '{method}': '{response.Content}' с  (HTTP код {response.StatusCode})");

            if (response.StatusCode == 0)
                throw new WebException("Server unavailable");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException($"{response.ResponseUri}");

            return new ApiResponse(response.Content);
        }

        /// <inheritdoc />
        /// <summary>
        /// Выполняет вход в учетную запись
        /// </summary>
        /// <param name="username">Имя пользователя для входа</param>
        /// <param name="password">Пароль для входа</param>
        public async Task SignIn(string username, string password)
        {
            var parameters = new Dictionary<string, string>
            {
                {"username", username},
                {"password", password}
            };

            var response = await CallAsync(SignInMethod, parameters);

            VerifyResponse(response);

            Username = response["username"];
            AccessToken = response["access_token"];

            logger.Info($"Успешная авторизация под именем пользователя '{Username}'");
        }

        /// <inheritdoc />
        /// <summary>
        /// Выполняет создание (регистрацию) новой учетные записи
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="email">Адрес электронной почты</param>
        /// <param name="password">Пароль</param>
        public async Task SignUp(string username, string email, string password)
        {
            var parameters = new Dictionary<string, string>
            {
                {"username", username},
                {"email", email},
                {"password", password}
            };

            var response = await CallAsync(SignUpMethod, parameters);

            VerifyResponse(response);

            logger.Info($"Выполнена регистрация пользователя '{username}'");
        }

        /// <inheritdoc />
        /// <summary>
        /// Выход из учетной записи
        /// </summary>
        public void LogOut()
        {
            logger.Info($"Выход из учетной записи '{Username}'...");

            Username = null;
            AccessToken = null;
        }

        /// <summary>
        /// Строит REST запрос с указанным методом и параметрами вызова
        /// </summary>
        /// <param name="methodName">Метод</param>
        /// <param name="parameters">Параметры вызова метода</param>
        /// <returns>Запрос в виде <see cref="RestRequest" /></returns>
        private RestRequest BuildRequest(string methodName, IDictionary<string, string> parameters)
        {
            var request = new RestRequest(methodName);

            if (parameters != null && parameters.Count > 0)
            {
                var paramInfo = string.Join(";", parameters.Select(x => x.Key + "=" + x.Value).ToArray());

                logger.Info($"Вызов метода '{methodName}' с параметрами {paramInfo}");

                foreach (var param in parameters)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }
            else
            {
                logger.Info($"Вызов метода {methodName} без параметров");
            }

            if (IsAuthorized)
            {
                request.AddHeader("Authorization", $"Bearer {AccessToken}");
            }

            return request;
        }

        /// <summary>
        /// Проверяет ответ сервера на ошибки и выбрасывает исключении при необходимости
        /// </summary>
        /// <param name="response">Ответ сервера</param>
        private static void VerifyResponse(ApiResponse response)
        {
            if (response.ContainsKey("error"))
            {
                var content = response["error"];

                int code = content["error_code"];
                string message = content["error_msg"];

                if (content.ContainsKey("validation_errors"))
                {
                    var pairs = content["validation_errors"]
                        .ToObject<dynamic[]>();

                    var validationErrors = new Dictionary<string, string>(pairs.Length);

                    foreach (var pair in pairs)
                        validationErrors.Add(pair.field.ToString(), pair.message.ToString());

                    throw new ApiException(message, code, validationErrors);
                }

                throw new ApiException(message, code);
            }
        }

        #endregion
    }
}