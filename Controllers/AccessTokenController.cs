using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Tasks_WEB_API.Authentifications;
using Tasks_WEB_API.Interfaces;
using Tasks_WEB_API.Models;


namespace Tasks_WEB_API.Controllers
{
	[Route("[controller]")]
	public class AccessTokenController : ControllerBase
	{

		private readonly DailyTasksMigrationsContext dataBaseMemoryContext;
		private readonly IJwtTokenService jwtTokenService;

		public AccessTokenController(DailyTasksMigrationsContext dataBaseMemoryContext, IJwtTokenService jwtTokenService)
		{

			this.dataBaseMemoryContext = dataBaseMemoryContext;
			this.jwtTokenService = jwtTokenService;
		}
		[HttpPost]
		public IActionResult Login(string email)
		{
			try
			{
				if(email is null)
				{
					return Conflict("Veuillez saisir une adresse mail valide");
				}
				var utilisateur = dataBaseMemoryContext.Utilisateurs.Where(u => u.Email.ToUpper().Equals(email.ToUpper())).FirstOrDefault();
				if (utilisateur.Email != email)
				{
					return Conflict("Cette adresse mail n'existe pas");
				}
				else if (utilisateur.Role != 0)
				{
					return Unauthorized("Vous ne disposez pas des droits suffisant !");
				}
				else
				{
					return Ok(jwtTokenService.GenerateJwtToken(utilisateur.Email));
				}
			}
			catch
			(Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur s'est produite ");
			}
		}
	}
}