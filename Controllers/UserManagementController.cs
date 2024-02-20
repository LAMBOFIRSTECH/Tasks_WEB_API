using Microsoft.AspNetCore.Mvc;
using Tasks_WEB_API.Interfaces;
namespace Tasks_WEB_API.Controllers;

[ApiController]
[Area("TasksDocumentation")]
[Route("api/v1.0/[area]")]

public class UserManagementController : ControllerBase
{
	private readonly IUtilisateurRepository utilisateurRepository;

	public UserManagementController(IUtilisateurRepository utilisateurRepository)
	{

		this.utilisateurRepository = utilisateurRepository;

	}

	/// <summary>
	/// Affiche la liste de tous les utilisateurs
	/// </summary>
	[HttpGet("~/GetUsers")]
	public async Task<ActionResult> GetUsers()
	{
		var users = await utilisateurRepository.GetUsers();
		return Ok(users);
	}

	/// <summary>
	/// Affiche les informations sur un utilisateur
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	[HttpGet("~/SelectUser/{ID:int}")]
	public async Task<ActionResult> GetUserById(int ID)
	{
		try
		{
			var utilisateur = await utilisateurRepository.GetUserById(ID);
			if (utilisateur != null)
			{
				return Ok(utilisateur);
			}
			return NotFound("Utilisateur non trouvé.");
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur s'est produite ");
		}
	}

	/// <summary>
	/// Créée un utilisateur
	/// </summary>
	/// <param name="identifiant"></param>
	/// <param name="nom"></param>
	/// <param name="mdp"></param>
	/// <param name="role"></param>
	/// <returns></returns>
	[HttpPost("~/CreateUser/")]
	public async Task<IActionResult> CreateUserById(int identifiant, string nom, string mdp, string role)
	{
		try
		{
			Utilisateur.Privilege privilege;
			if (!Enum.TryParse(role, true, out privilege))
			{
				return BadRequest("Le rôle spécifié n'est pas valide.");
			}
			Utilisateur newUtilisateur = new() { ID = identifiant, Nom = nom, Pass = mdp, Role = privilege };
			var listUtilisateurs = await utilisateurRepository.GetUsers();
			foreach (var item in listUtilisateurs)
			{

				if (item.Nom == nom && item.Role == privilege)
				{

					return Conflict("Cet utilisateur est déjà présent");
				}
			}
			await utilisateurRepository.CreateUserById(newUtilisateur);

			return Ok("La ressource a bien été créée");
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "Erreur lors de la création d'un utilisateur");
		}
	}

	/// <summary>
	/// Supprime un utilisateur
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	[HttpDelete("~/DeleteUser/{ID:int}")]
	public async Task<IActionResult> DeleteUserById(int ID)
	{
		var utilisateur = await utilisateurRepository.GetUserById(ID);
		try
		{
			if (utilisateur == null)
			{
				return NotFound($"L'utilisateur id=[{ID}] n'a pas été trouvé dans le contexte de base de données");
			}

			await utilisateurRepository.DeleteUserById(ID);

			return Ok("La donnée a bien été supprimée");
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError,
					  "Error deleting data");
		}

	}

	/// <summary>
	///  Met à jour les informations d'un utilisateur
	/// </summary>
	/// <param name="utilisateur"></param>
	/// <returns></returns>
	[HttpPut("~/UpdateUser")]
	public async Task<IActionResult> UpdateUser([FromBody] Utilisateur utilisateur)
	{
		try
		{
			var item = await utilisateurRepository.GetUserById(utilisateur.ID);
			if (item is null)
			{
				return NotFound($"Cet utilisateur n'existe plus dans le contexte de base de données");
			}
			if (item.ID == utilisateur.ID)
			{
				await utilisateurRepository.UpdateUser(utilisateur);
			}
			return Ok($"Les infos de l'utilisateur [{item.ID}] ont bien été modifiées.");
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError,
					  "Error deleting data");
		}
	}
}
