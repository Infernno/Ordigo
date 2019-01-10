using System.Collections.Generic;
using System.Threading.Tasks;
using Ordigo.API.Infrastructure;

namespace Ordigo.API.Contracts
{
    /// <summary>
    /// Интерфейс вызова API
    /// </summary>
    public interface IApiInvoke
    {
        /// <summary>
        /// Асинхронный вызов API метода сервера
        /// </summary>
        /// <param name="method">Название метода</param>
        /// <param name="parameters">Параметры вызова метода в виде словаря</param>
        /// <returns></returns>
        Task<ApiResponse> CallAsync(string method, IDictionary<string, string> parameters);
    }
}
