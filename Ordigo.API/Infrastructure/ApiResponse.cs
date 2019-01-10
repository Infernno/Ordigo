using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Ordigo.API.Infrastructure
{
    /// <summary>
    /// Класс, содержащий ответ от сервера в формате JSON
    /// </summary>
    public sealed class ApiResponse
    {
        #region Constructor

        public ApiResponse(JToken token)
        {
            mToken = token;
        }

        public ApiResponse(JToken token, string rawJson)
        {
            mToken = token;
            RawJson = rawJson;
        }

        public ApiResponse(string rawJson) : this(JToken.Parse(rawJson), rawJson)
        {
        }

        #endregion

        #region Fields

        /// <summary>
        /// JSON токен, содержащий в себе ответ от сервера
        /// </summary>
        private readonly JToken mToken;

        #endregion
    
        #region Properties

        /// <summary>
        /// Сырой JSON
        /// </summary>
        public string RawJson { get; }

        /// <summary>
        /// Проверяет, пустой ли токен 
        /// </summary>
        public bool HasToken => mToken != null && mToken.HasValues;

        /// <summary>
        /// Индексатор, который возвращает внутренний токен по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public ApiResponse this[object key]
        {
            get
            {
                if (mToken is JArray && key is string)
                    return null;

                var token = mToken[key];

                return token != null && token.Type != JTokenType.Null
                    ? new ApiResponse(token)
                    : null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Проверяет, существует ли внутренний токен с указанным ключем
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            if (!(mToken is JObject))
                return false;

            var token = mToken[key];

            return token != null && token.Type != JTokenType.Null;
        }

        /// <summary>
        /// Выполняет преобразование токена в указанный тип
        /// </summary>
        /// <typeparam name="T">Тип, в который будет преобразован токен</typeparam>
        /// <returns></returns>
        public T ToObject<T>()
        {
            return mToken.ToObject<T>();
        }

        #endregion

        #region System types

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator bool(ApiResponse response)
        {
            return response != null && response == 1;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator bool? (ApiResponse response)
        {
            return response == null ? (bool?)null : response == 1;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator long(ApiResponse response)
        {
            return (long)response.mToken;
        }

        /// <summary>
        ///  Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        ///  Результат преобразования.
        /// </returns>
        public static implicit operator long? (ApiResponse response)
        {
            return response != null ? (long?)response.mToken : null;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator ulong(ApiResponse response)
        {
            return (ulong)response.mToken;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator ulong? (ApiResponse response)
        {
            return response != null ? (ulong?)response.mToken : null;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator float(ApiResponse response)
        {
            return (float)response.mToken;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator float? (ApiResponse response)
        {
            return response != null ? (float?)response.mToken : null;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        ///  Результат преобразования.
        /// </returns>
        public static implicit operator decimal(ApiResponse response)
        {
            return (decimal)response.mToken;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator decimal? (ApiResponse response)
        {
            return response != null ? (decimal?)response.mToken : null;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator uint(ApiResponse response)
        {
            return (uint)response.mToken;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator uint? (ApiResponse response)
        {
            return response != null ? (uint?)response.mToken : null;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator int(ApiResponse response)
        {
            return (int)response.mToken;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator int? (ApiResponse response)
        {
            return response != null ? (int?)response.mToken : null;
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response"> Ответ от сервера </param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator string(ApiResponse response)
        {
            return response == null ? null : WebUtility.HtmlDecode((string)response.mToken);
        }

        /// <summary>
        ///  Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response"> Ответ от сервера </param>
        /// <returns>
        ///  Результат преобразования.
        /// </returns>
        public static implicit operator DateTime? (ApiResponse response)
        {
            var dateStringValue = response?.ToString();

            if (string.IsNullOrWhiteSpace(dateStringValue)
                || !long.TryParse(dateStringValue, out var unixTimeStamp)
                || unixTimeStamp <= 0)
                return null;

            return TimestampToDateTime(unixTimeStamp);
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response"> Ответ от сервера </param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator DateTime(ApiResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var dateStringValue = response.ToString();

            if (string.IsNullOrWhiteSpace(dateStringValue))
                throw new ArgumentException("Пустое значение невозможно преобразовать в дату", nameof(response));

            if (!long.TryParse(dateStringValue, out var unixTimeStamp) || unixTimeStamp <= 0)
                throw new ArgumentException("Невозможно преобразовать в дату", nameof(response));

            return TimestampToDateTime(unixTimeStamp);
        }

        /// <summary>
        /// Timestamps to date time
        /// </summary>
        /// <param name="unixTimeStamp">The unix time stamp</param>
        /// <returns> </returns>
        public static DateTime TimestampToDateTime(long unixTimeStamp)
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return dt.AddSeconds(unixTimeStamp);
        }

        /// <summary>
        /// Выполняет неявное преобразование из <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="response">Ответ от сервера</param>
        /// <returns>
        /// Результат преобразования.
        /// </returns>
        public static implicit operator Uri(ApiResponse response)
        {
            return Uri.TryCreate(response, UriKind.Absolute, out var uriResult) ? uriResult : null;
        }

        #endregion
    }
}