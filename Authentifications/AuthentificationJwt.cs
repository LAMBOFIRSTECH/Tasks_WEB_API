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
	public class AuthentificationJwt : AuthenticationHandler<AuthenticationSchemeOptions>, IAuthorizationFilter
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

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			throw new NotImplementedException();
		}

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }
    }


}

