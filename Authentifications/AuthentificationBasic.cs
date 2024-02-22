using Microsoft.AspNetCore.Mvc;
using Tasks_WEB_API.Interfaces;
using Tasks_WEB_API.Models;
using System;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Tasks_WEB_API.Repositories
{
	public class AuthentificationBasic : AuthenticationHandler<AuthenticationSchemeOptions>,IAuthorizationFilter//, IAuthentificationRepository //,IUtilisateurRepository,ITacheRepository
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

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
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
					var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
					var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
					var username = credentials[0];
					var password = credentials[1];

					// Votre logique d'authentification ici
					if (await BasicAuthentification(username, password))
					{
						// Créer un ClaimsPrincipal avec le nom d'utilisateur
						var claims = new[] { new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username) };
						var identity = new System.Security.Claims.ClaimsIdentity(claims, Scheme.Name);
						var principal = new System.Security.Claims.ClaimsPrincipal(identity);

						// Assigner le principal à la propriété Principal de l'objet context
						var ticket = new AuthenticationTicket(principal, Scheme.Name);
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

	
		
		private async Task<bool> BasicAuthentification(string username, string password) //a deboguer
		{
			// Votre logique d'authentification réelle asynchrone ici
			// Par exemple, vérifier les informations d'identification dans une base de données
			// ou un autre magasin de données

			// Simulons une attente asynchrone ici pour une démonstration
			var users = dataBaseMemoryContext.Utilisateurs.ToList();
			foreach (var item in users)
			{
				if (item.Nom == username && item.Pass == password)
				{
					throw new NotImplementedException();

				}
			}
			await Task.Delay(1000);

			return username == "admin" && password == "password";
		}


	}


}
