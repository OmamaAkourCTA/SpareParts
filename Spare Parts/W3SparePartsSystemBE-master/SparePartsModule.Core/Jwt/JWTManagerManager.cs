using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SparePartsModule.Core
{
    public class JWTManagerManager : IJWTManagerManager
    {
        private readonly IConfiguration _configuration;
        public JWTManagerManager(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public TokenOutputDto GenerateToken(TokenInputDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            //if (string.IsNullOrEmpty(model.MobileNumber))
            //{
            //    throw new ArgumentNullException(nameof(model.MobileNumber));
            //}
            var expiryDateTime = DateTime.UtcNow.AddMonths(1);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"] ?? "NoKeysYallah");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                        new Claim(ClaimTypes.Name, model.FullName??"No Name"),
                        new Claim(ClaimTypes.MobilePhone, model.MobileNumber)
                }),
                Expires = expiryDateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
           
            return new TokenOutputDto(tokenHandler.WriteToken(token), string.Empty, expiryDateTime);
        }
    }
}

