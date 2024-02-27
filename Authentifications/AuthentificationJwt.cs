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
	public class AuthentificationJwt : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		private readonly DailyTasksMigrationsContext dataBaseMemoryContext;
		public AuthentificationJwt(DailyTasksMigrationsContext dataBaseMemoryContext, IOptionsMonitor<AuthenticationSchemeOptions> options,
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
				var username = "";
				var password = "";
				if (await IsValidCredentials(username, password))

				{
					return AuthenticateResult.NoResult();//pb
				}
				return AuthenticateResult.NoResult();//pb
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