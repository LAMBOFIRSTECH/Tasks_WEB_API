using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tasks_WEB_API.Models;

namespace Tasks_WEB_API.Controllers;

[ApiController]
[Route("api/v1.0/[controller]")]
public class UserManagementController : ControllerBase
{
    private readonly DailyTasksMigrationsContext _content;

    private readonly ILogger<UserManagementController> _logger;
    public UserManagementController(ILogger<UserManagementController> logger, DailyTasksMigrationsContext context)
    {
        _logger = logger;
        _content = context;

    }

    /// <summary>
    /// Affiche la liste de tous les utilisateurs
    /// </summary>
    [HttpGet("~/GetUsers")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var UsersContextDB = await _content.Utilisateurs.ToListAsync();
            if (UsersContextDB.Any())
            {
                return Ok(UsersContextDB);
            }
            else
            {
                return NotFound("Pas de donnée utilisateur présente dans le contexte de base de données.");
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
    /// <param name="ID"></param>
    /// <returns></returns>
    [HttpGet("~/SelectUser/{ID:int}")]
    public async Task<IActionResult> SelectUser(int ID)
    {
        try
        {
            var utilisateur = await _content.Utilisateurs.FindAsync(ID);
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
    /// 
    /// </summary>
    /// <param name="identifiant"></param>
    /// <param name="nom"></param>
    /// <returns></returns>
    [HttpPost("~/CreateUser/{identifiant:int}/{nom}")]
    public async Task<IActionResult> CreateUser(int identifiant, string nom)
    {
        try
        {
            var DataBaseContext = await _content.Utilisateurs.ToListAsync();

            if (DataBaseContext.Any(u => u.ID == identifiant))
            {
                return Conflict("Cet utilisateur est déjà présent.");
            }
            Utilisateur utilisateur = new Utilisateur() { ID = identifiant, Nom = nom };

            // Enregistrement du nouvel utilisateur dans le contexte de base de données.
            await _content.Utilisateurs.AddAsync(utilisateur);
            await _content.SaveChangesAsync();
            // Obtention de la liste mise à jour des utilisateurs après la création
            var UsersContextDB = await _content.Utilisateurs.ToListAsync();

            return Ok("La ressource a bien été créée");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
        }
    }

    /// <summary>
    /// On veut supprimer une tache à l'aide de son Matricule.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    [HttpDelete("~/DeleteUser/{ID:int}")]
    public async Task<IActionResult> DeleteUser(int ID)
    {
        var utilisateur = await _content.Utilisateurs.FindAsync(ID);
        try
        {
            if (utilisateur == null)
            {
                return NotFound($"L'utilisateur [{ID}] n'existe plus dans le contexte de base de données");
            }
            _content.Utilisateurs.Remove(utilisateur);
            await _content.SaveChangesAsync();

            var users = _content.Utilisateurs.ToListAsync();
            users.Result.Any();

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
    /// <returns></returns>
    [HttpPut("~/UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] Utilisateur utilisateur)
    {
        try
        {
            var item = await _content.Utilisateurs.FindAsync(utilisateur.ID);

            if (item is null)
            {
                return NotFound($"Cet utilisateur n'existe plus dans le contexte de base de données");
            }
            if (item.ID == utilisateur.ID)
            {
                item.Nom = utilisateur.Nom;
                await _content.SaveChangesAsync();

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
