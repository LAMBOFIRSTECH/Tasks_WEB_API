using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Tasks_WEB_API.Models;

namespace Tasks_WEB_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksManagementController : ControllerBase
{
    private readonly DailyTasksMigrationsContext _content;

    private readonly ILogger<TasksManagementController> _logger;
    public TasksManagementController(ILogger<TasksManagementController> logger, DailyTasksMigrationsContext context)
    {
        _logger = logger;
        _content = context;

        _content.Database.EnsureCreated();

    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("~/GetTasks")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var TasksContextDB = await _content.Taches.ToListAsync();
            if (TasksContextDB.Any())
            {
                return Ok(TasksContextDB);
            }
            else
            {
                return NotFound("Pas de données tache présentent dans le contexte de base de données.");
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Erreur : lors de la collecte de donnée dans le contexte de base de donnée.");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Matricule"></param>
    /// <returns></returns>
    [HttpGet("~/SelectTask/{Matricule:int}")]
    public async Task<IActionResult> SelectUser(int Matricule)
    {
        try
        {
            var tache = await _content.Taches.FindAsync(Matricule);
            if (tache != null)
            {
                return Ok(tache);
            }
            return NotFound("Tache non trouvé.");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur s'est produite ");
        }
    }

    [HttpPost("~/CreateTask")]
    public async Task<IActionResult> CreateTask([FromBody] Tache tache)
    {
        try
        {
            var DataBaseContext = await _content.Taches.ToListAsync();

            if (DataBaseContext.Any(t => t.Matricule == tache.Matricule))
            {
                return Conflict("Cette tache est déjà présente.");
            }
            Tache task = new Tache() { Matricule = tache.Matricule, Titre = tache.Titre, Summary = tache.Summary, DateH = tache.DateH };

            // Enregistrement du nouvel utilisateur dans le contexte de base de données.
            await _content.Taches.AddAsync(task);
            await _content.SaveChangesAsync();
            // Obtention de la liste mise à jour des utilisateurs après la création
            var TasksContextDB = await _content.Taches.ToListAsync();

            return Ok("La ressource a bien été créée");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Matricule"></param>
    /// <returns></returns>
    [HttpDelete("~/DeleteTask/{Matricule:int}")]
    public async Task<IActionResult> DeleteUser(int Matricule)
    {
        var tache = await _content.Taches.FindAsync(Matricule);
        try
        {
            if (tache == null)
            {
                return NotFound($"La tache de matricule : [{Matricule}] n'existe plus dans le contexte de base de données");
            }
            _content.Taches.Remove(tache);
            await _content.SaveChangesAsync();

            var taches = _content.Taches.ToListAsync();
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
    /// 
    /// </summary>
    /// <param name="tache"></param>
    /// <returns></returns>
    [HttpPut("~/UpdateTask")]
    public async Task<IActionResult> UpdateTask([FromBody] Tache tache)
    {
        try
        {
            var item = await _content.Taches.FindAsync(tache.Matricule);

            if (item is null)
            {
                return NotFound($"Cet utilisateur n'existe plus dans le contexte de base de données");
            }
            if (item.Matricule == tache.Matricule)
            {
                item.Titre = tache.Titre;
                item.Summary = tache.Summary;
                item.DateH = tache.DateH;
                await _content.SaveChangesAsync();

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
