using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Tasks_WEB_API.Authentifications
{
	public class JwtBearerAuthentification : AuthenticationHandler<JwtBearerOptions>
	{

		public JwtBearerAuthentification(IOptionsMonitor<JwtBearerOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder,
		ISystemClock clock)
		: base(options, logger, encoder, clock)
		{
			
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{

			if (!Request.Headers.ContainsKey("Authorization"))
				return Task.FromResult(AuthenticateResult.Fail("Authorization header missing"));
			try
			{
				var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
				if (!authHeader.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
				{
					return Task.FromResult(AuthenticateResult.Fail("Invalid authentication scheme"));
				}
				// Récupérer le jeton JWT à partir de l'en-tête d'autorisation
				var jwtToken = authHeader.Parameter;
				var tokenValidationParameters = Options.TokenValidationParameters;
				
				
				if (tokenValidationParameters == null)
					return Task.FromResult(AuthenticateResult.Fail("Token validation parameters are not configured"));

				var tokenHandler = new JwtSecurityTokenHandler();
				SecurityToken securityToken;// ici ?
				var principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out securityToken);

				// Créer un ticket d'authentification réussi avec le principal
				var ticket = new AuthenticationTicket(principal, Scheme.Name);
				return Task.FromResult(AuthenticateResult.Success(ticket));

			}
			catch (Exception ex)
			{
				return Task.FromResult(AuthenticateResult.Fail($"Authentication failed: {ex.Message}"));
			}
		}


	}

}
