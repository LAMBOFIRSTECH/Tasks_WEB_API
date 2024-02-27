using Tasks_WEB_API.Models;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Tasks_WEB_API.Repositories
{
	public class AuthentificationJwt : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		private readonly DailyTasksMigrationsContext dataBaseMemoryContext;
		private readonly IConfiguration configuration;
		public AuthentificationJwt(DailyTasksMigrationsContext dataBaseMemoryContext, IOptionsMonitor<AuthenticationSchemeOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder,
		ISystemClock clock, IConfiguration configuration)
		: base(options, logger, encoder, clock)
		{
			this.dataBaseMemoryContext = dataBaseMemoryContext;
			this.configuration = configuration;
		}
		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			var jwtSettings = configuration.GetSection("JwtSettings");
			var SecretKey = jwtSettings["SecretKey"];
			try
			{
				var key = new RandomUserSecret().GenerateRandomKey(64);
				var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));


				// Configuration des paramètres de validation
				var tokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = securityKey, // Définir la clé de signature de l'émetteur
																	 // Autres paramètres de validation...
				ValidateIssuer = true,
				ValidIssuer = jwtSettings["Issuer"],
				ValidateAudience = true,
				ValidAudience = jwtSettings["Audience"]

			};

			// Valider le jeton JWT
			//var principal = ValidateJwtToken(tokenValidationParameters); // Mettez ici votre logique pour valider le jeton JWT

			// Créer un jeton d'authentification réussi
			// var ticket = new AuthenticationTicket(principal, Scheme.Name);
			// return AuthenticateResult.Success(ticket);
			return AuthenticateResult.NoResult();
		}
			catch (Exception ex)
			{
				// En cas d'erreur, retourner un résultat d'authentification en échec avec l'erreur
				return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
			}

	// if (!Request.Headers.ContainsKey("Authorization"))
	// 	return AuthenticateResult.Fail("Authorization header missing");

	// try
	// {
	// 	var username = "";
	// 	var password = "";
	// 	if (await IsValidCredentials(username, password))

	// 	{
	// 		return AuthenticateResult.NoResult();//pb
	// 	}
	// 	return AuthenticateResult.NoResult();//pb
	// }
	// catch (Exception ex)
	// {
	// 	return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
	// }
}
private async Task<bool> IsValidCredentials(string username, string password)
{
	var utilisateur = dataBaseMemoryContext.Utilisateurs.FirstOrDefault(u => u.Nom == username);
	if (utilisateur != null)
	{
		return utilisateur.ChechHashPassword(password);
	}
	await Task.Delay(1000);
	return false;
}
	}
}