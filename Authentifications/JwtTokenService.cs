using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tasks_WEB_API.Models;
using Tasks_WEB_API.Interfaces;
using Microsoft.OpenApi.Extensions;

namespace Tasks_WEB_API.Authentifications
{
	public class JwtTokenService : IJwtTokenService
	{
		private readonly DailyTasksMigrationsContext dataBaseMemoryContext;
		private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

		public JwtTokenService(DailyTasksMigrationsContext dataBaseMemoryContext, Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			this.dataBaseMemoryContext = dataBaseMemoryContext;
			this.configuration = configuration;
		}
		public string GetSigningKey()
		{
			var JwtSettings = configuration.GetSection("JwtSettings");
			int secretKey = int.Parse(JwtSettings["SecretKey"]);
			var randomSecretKey = new RandomUserSecret();
			string signingKey = randomSecretKey.GenerateRandomKey(secretKey);
			return signingKey;
		}
		public string GenerateJwtToken(string email)
		{
			var utilisateur = dataBaseMemoryContext.Utilisateurs.Where(u => u.Email.ToUpper().Equals(email.ToUpper())).FirstOrDefault();
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSigningKey()));
			var tokenHandler = new JwtSecurityTokenHandler();

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] {
					new Claim(ClaimTypes.Name, utilisateur.Nom),
					new Claim(ClaimTypes.Email, utilisateur.Email),
					new Claim(ClaimTypes.Role, utilisateur.Role.ToString())
					}
				),
				
				Expires = DateTime.UtcNow.AddMinutes(5),
				SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
				Audience = configuration.GetSection("JwtSettings")["Audience"],
				Issuer = configuration.GetSection("JwtSettings")["Issuer"],
			};
			var tokenCreation = tokenHandler.CreateToken(tokenDescriptor);
			var token = tokenHandler.WriteToken(tokenCreation);
			return token;
		}
	}
}
