using Tasks_WEB_API.Models;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
namespace Tasks_WEB_API.Repositories
{
	public class AuthentificationBasic : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		private readonly DailyTasksMigrationsContext dataBaseMemoryContext;
		public AuthentificationBasic(DailyTasksMigrationsContext dataBaseMemoryContext, IOptionsMonitor<AuthenticationSchemeOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder,
		ISystemClock clock)
		: base(options, logger, encoder, clock)
		{
			this.dataBaseMemoryContext = dataBaseMemoryContext;
		}
		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.ContainsKey("Authorization"))
				return AuthenticateResult.Fail("Authorization header missing");

			try
			{
				var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
				if (authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
				{
					var credentialBytes = Convert.FromBase64String(authHeader.Parameter);//header du token
					var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
					var username = credentials[0];
					var password = credentials[1];
					// On va implémenter la logique d'authentification ici
					if (await IsValidCredentials(username, password))
					{
						// Créer un ClaimsPrincipal avec le nom et le privilege de l'utilisateur playload du token
						var claims = new[]
						{
						new Claim(ClaimTypes.Name, username),
						new Claim(ClaimTypes.Role, Utilisateur.Privilege.UserX.ToString())
						};

						var identity = new ClaimsIdentity(claims, Scheme.Name);
						var principal = new ClaimsPrincipal(identity);

						// Assigner le principal à la propriété Principal de l'objet context
						var ticket = new AuthenticationTicket(principal, Scheme.Name);//signature header http --> Authorization: Basic am9objpwYXNzd29yZA==

						return AuthenticateResult.Success(ticket);
					}
					else
					{
						return AuthenticateResult.Fail("Invalid username or password");
					}
				}
				else
				{
					return AuthenticateResult.NoResult();
				}
			}
			catch (Exception ex)
			{
				return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
			}
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