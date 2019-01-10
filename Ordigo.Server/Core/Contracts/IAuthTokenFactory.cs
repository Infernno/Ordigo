using System.Collections.Generic;
using System.Security.Claims;

namespace Ordigo.Server.Core.Contracts
{
    /// <summary>
    /// Фабрика, создающая токен авторизации для пользователя
    /// </summary>
    public interface IAuthTokenFactory
    {
        /// <summary>
        /// Создает токен авторизации с указанным набором <see cref="Claim"/> 
        /// </summary>
        /// <param name="claims">Набор <see cref="Claim"/></param>
        /// <returns>Токен в виде строки</returns>
        string Generate(IEnumerable<Claim> claims);
    }
}
