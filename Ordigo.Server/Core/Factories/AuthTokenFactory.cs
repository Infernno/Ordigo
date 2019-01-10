using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ordigo.Server.Core.Contracts;

namespace Ordigo.Server.Core.Factories
{
    /// <inheritdoc />
    /// <summary>
    /// Фабрика, создающая токен авторизации для пользователя
    /// </summary>
    public class AuthTokenFactory : IAuthTokenFactory
    {
        #region Constructor

        public AuthTokenFactory(IConfiguration configuration)
        {
            Issuer = configuration["Jwt:Issuer"];
            EncryptionKey = configuration["Jwt:Key"];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Издатель токена
        /// </summary>
        public string Issuer { get; }

        /// <summary>
        /// Ключ для шифрования подписи токена
        /// </summary>
        public string EncryptionKey { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        /// Создает токен авторизации с указанным набором <see cref="T:System.Security.Claims.Claim" />
        /// </summary>
        /// <param name="claims">Набор <see cref="T:System.Security.Claims.Claim" /></param>
        /// <returns>Токен в виде строки</returns>
        public string Generate(IEnumerable<Claim> claims)
        {
            var key = Generate();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                Issuer,
                // audience: Audience,
                claims: claims,
                notBefore: now,
                // expires: now.Add(TimeSpan.FromSeconds(LifeTime)),
                signingCredentials: creds);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwt);

            return token;
        }

        /// <summary>
        /// Преобразует заданный ключ из строки в симметричный ключ в виде <see cref="SymmetricSecurityKey"/>
        /// </summary>
        /// <returns></returns>
        private SymmetricSecurityKey Generate()
        {
            var bytes = Encoding.ASCII.GetBytes(EncryptionKey);
            var key = new SymmetricSecurityKey(bytes);

            return key;
        }

        #endregion
    }
}