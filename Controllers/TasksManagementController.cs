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
    private readonly TasksManagementContext _content;

    private readonly ILogger<TasksManagementController> _logger;
    public TasksManagementController(ILogger<TasksManagementController> logger, TasksManagementContext context)
    {
        _logger = logger;
        _content = context;
    }

    // public async Task GetUtilisateur()
    // {

    // }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Liste de tous les utilisateurs </returns>
    [Route("~/GetUtilisateurs")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {

        List<Utilisateur> maListe = new List<Utilisateur>()
        {
            new Utilisateur(){Matricule = "01User1", Nom = "lambo"},

            new Utilisateur(){Matricule = "02User2", Nom = "artur"}

        };
        _content.Database.EnsureCreated();

        foreach (var item in maListe)
        {

            _content.Utilisateurs.Add(item);
            _content.SaveChanges();
        }

        var utilisateurs = await _content.Utilisateurs
            .OrderBy(user => user.Matricule)
            .ToListAsync();

        if (utilisateurs.Any())
        {
            return Ok(utilisateurs);
        }
        else
        {
            return NotFound("pas de données");
        }
    }


    // [HttpPost]
    // public IActionResult AddContains()
    // {
    //     var tache = "";
    //     return tache;
    // }

    /// <summary>
    /// On veut supprimer une tache à l'aide de son ID.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    [HttpDelete("{ID}")]
    public async Task<IActionResult> Delete(int ID)
    {
        var item = await _content.Taches.FindAsync(ID);
        if (item is null)
        {
            return NotFound();
        }
        _content.Taches.Remove(item);
        await _content.SaveChangesAsync();
        return NoContent();
    }
}
