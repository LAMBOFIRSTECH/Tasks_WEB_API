using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Tasks_WEB_API.Models;

namespace Tasks_WEB_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserManagementController : ControllerBase
{
    private readonly DailyTasksContext _content;

    private readonly ILogger<UserManagementController> _logger;
    public UserManagementController(ILogger<UserManagementController> logger, DailyTasksContext context)
    {
        _logger = logger;
        _content = context;

        _content.Database.EnsureCreated();
        List<Utilisateur> maListe = new List<Utilisateur>()
        {
            new Utilisateur(){ID = 11, Nom = "lambo"},

            new Utilisateur(){ID = 22, Nom = "artur"}

        };

        foreach (var item in maListe)
        {
            _content.Utilisateurs.Add(item);
            _content.SaveChangesAsync(); //c4est grace à ceci qu'on peut actualisé le swagger pour la liste des users de façon async
        }

        List<Tache> taches = new List<Tache>()
        {
            new Tache(){ID="01a", Titre = "faire un audit digital",Summary="prppprp",Date=DateTime.Now},
            new Tache(){ID="01b", Titre = "demoulage",Summary="oooooooo",Date=DateTime.Now}
        };
        foreach (var item in taches)
        {
            _content.Taches.Add(item);
            _content.SaveChangesAsync();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Liste de tous les utilisateurs </returns>
    [Route("~/GetUsersList")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {

        try
        {
            var utilisateurs = await _content.Utilisateurs.ToListAsync();
            if (utilisateurs.Any())
            {
                return Ok(utilisateurs);
            }
            else
            {
                return NotFound("pas de données");
            }

        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
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
        var item = await _content.Utilisateurs.FindAsync(ID);
        if (item is null)
        {
            return NotFound();
        }
        return Ok(item);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>

    [HttpPost("~/CreateUser/")]
    public async Task<IActionResult> AddContains()
    {
        var item = await _content.Utilisateurs.ToListAsync();

        if (item is null)
        {
            return NotFound();
        }
        return Ok();
    }
    [HttpPut("~/UpdateUser/{ID:int}")]
    public async Task<IActionResult> UpdateContains()
    {

        var item = await _content.Utilisateurs.ToListAsync();

        if (item is null)
        {
            return NotFound();
        }
        return Ok();
    }

    /// <summary>
    /// On veut supprimer une tache à l'aide de son Matricule.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    [HttpDelete("~/DeleteUser/{ID:int}")]

    public async Task<IActionResult> Delete(int ID)
    {
        var item = await _content.Utilisateurs.FindAsync(ID);
        try
        {
            if (item is null)
            {
                return NotFound();
            }

            _content.Utilisateurs.Remove(item);
            await _content.SaveChangesAsync();
            return Ok("La donnée a bien été supprimée");
        }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                      "Error deleting data");
        }

    }


    // autre controller
}
