using System;
using System.Text.Json.Serialization;

namespace SparePartsModule.Infrastructure.ViewModels
{
    public class TokenOutputDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="refreshToken"></param>
        /// <param name="expiryDateTime"></param>
        public TokenOutputDto(string token, string refreshToken, DateTime expiryDateTime)
        {
            this.Token = token;
            this.RefreshToken = refreshToken;
            this.ExpiryDateTime = expiryDateTime;
        }
        public string Token { get; }
        public string RefreshToken { get; }
        public DateTime ExpiryDateTime { get; }
    }
}

