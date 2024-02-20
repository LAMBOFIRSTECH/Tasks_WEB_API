using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tasks_WEB_API.Interfaces;
namespace Tasks_WEB_API.Controllers;

[ApiController]
//[Area("TasksDocumentation")]
[Route("api/v1.0/[Controller]")]
public class TasksManagementController : ControllerBase
{
	private readonly ITacheRepository tacheRepository;


	public TasksManagementController(ITacheRepository tacheRepository)
	{

		this.tacheRepository = tacheRepository;

	}

	/// <summary>
	/// Affiche la liste de toutes les taches
	/// </summary>
	/// <returns></returns>
	[HttpGet("~/GetTasks")]
	public async Task<IActionResult> Get()
	{
		var taches = await tacheRepository.GetTaches();
		return Ok(taches);
	}

	/// <summary>
	/// Affiche les informations sur une tache précise
	/// </summary>
	/// <param name="Matricule"></param>
	/// <returns></returns>
	[HttpGet("~/SelectTask/{Matricule:int}")]
	public async Task<IActionResult> SelectTask(int Matricule)
	{
		try
		{
			var tache = await tacheRepository.GetTaskById(Matricule);
			if (tache != null)
			{
				return Ok(tache);
			}
			return NotFound("tache non trouvée.");
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, $"Une erreur s'est produite dans la recherche de la ressource {Matricule}.");
		}

	}

	/// <summary>
	/// Crée une tache 
	/// </summary>
	/// <param name="tache"></param>
	/// <returns></returns>
	[HttpPost("~/CreateTask")]
	public async Task<IActionResult> CreateTask([FromBody] Tache tache)
	{
		try
		{

			Tache newTache = new()
			{
				Matricule = tache.Matricule,
				Titre = tache.Titre,
				Summary = tache.Summary,
				TasksDate = new()
				{
					EndDateH = tache.TasksDate.EndDateH,
					StartDateH = tache.TasksDate.StartDateH
				}
			};
			var listTaches = await tacheRepository.GetTaches();
			foreach (var item in listTaches)
			{

				if (item.Matricule == tache.Matricule)
				{

					return Conflict("Cette tache est déjà présente");
				}
			}
			await tacheRepository.CreateTaskById(newTache);
			return Ok("La ressource a bien été créée");
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "Erreur lors de la création de la ressource");
		}
	}

	/// <summary>
	/// Supprime une tache
	/// </summary>
	/// <param name="Matricule"></param>
	/// <returns></returns>
	[HttpDelete("~/DeleteTask/{Matricule:int}")]
	public async Task<IActionResult> DeleteUser(int Matricule)
	{
		var tache = await tacheRepository.Taches.FindAsync(Matricule);
		try
		{
			if (tache == null)
			{
				return NotFound($"La tache de matricule : [{Matricule}] n'existe plus dans le contexte de base de données");
			}
			tacheRepository.Taches.Remove(tache);
			await tacheRepository.SaveChangesAsync();

			var taches = tacheRepository.Taches.ToListAsync();
			taches.Result.Any();

			return Ok("La donnée a bien été supprimée");
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError,
					  "Error deleting data");
		}
	}

	/// <summary>
	/// Met à jour les informations d'une tache
	/// </summary>
	/// <param name="tache"></param>
	/// <returns></returns>
	[HttpPut("~/UpdateTask")]
	public async Task<IActionResult> UpdateTask([FromBody] Tache tache)
	{
		try
		{
			var item = await tacheRepository.Taches.FindAsync(tache.Matricule);

			if (item is null)
			{
				return NotFound($"Cette tache n'existe plus dans le contexte de base de données");
			}
			if (item.Matricule == tache.Matricule)
			{
				item.Titre = tache.Titre;
				item.Summary = tache.Summary;
				item.TasksDate = new Tache.DateH() { StartDateH = tache.TasksDate.StartDateH, EndDateH = tache.TasksDate.EndDateH };
				await tacheRepository.SaveChangesAsync();

			}
			return Ok($"Les infos de la tache [{item.Matricule}] ont bien été modifiées.");
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError,
					  "Error deleting data");
		}
	}
}
