using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Tasks_WEB_API.Models;

namespace Tasks_WEB_API.Controllers;

[ApiController]
[Route("api/v1.0/[controller]")]
public class UserManagementController : ControllerBase
{
    private readonly DailyTasksContext _content;

    private readonly ILogger<UserManagementController> _logger;
    public UserManagementController(ILogger<UserManagementController> logger, DailyTasksContext context)
    {
        _logger = logger;
        _content = context;
    }
    /// <summary>
    /// Cette méthode permet de créer des utilisateurs et les sauvegarder dans le contexte de base de données.
    /// </summary>
    /// <returns>listeUtilisateurs</returns>
    [NonAction]
    public async Task<List<Utilisateur>> UsersListe()
    {
        _content.Database.EnsureCreated();
        try
        {
            List<Utilisateur> maListe = new List<Utilisateur>()
                {
                    new Utilisateur(){ID = 11, Nom = "lambo"},

                    new Utilisateur(){ID = 22, Nom = "artur"}
                };
            foreach (var item in maListe)
            {
                _content.Utilisateurs.Add(item);
            }
            await _content.SaveChangesAsync();
        }
        catch (Exception)
        {
            Console.WriteLine("Error");
        }
        List<Utilisateur> listeUtilisateurs = await _content.Utilisateurs.ToListAsync();
        return listeUtilisateurs;
    }

    /// <summary>
    /// Affiche la liste de tous les utilisateurs
    /// </summary>
    /// <returns> </returns>
    [Route("~/GetUsersList")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var UsersContextDB = await UsersListe();
            if (UsersContextDB.Any())
            {
                return Ok(UsersContextDB);
            }
            else
            {
                return NotFound("Pas de données utilisateurs présent dans le contexte de base de données.");
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error : retrieving data from the database context");
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
        var DataBaseContext = await UsersListe();
        foreach (var elt in DataBaseContext)
        {
            if (elt.ID == ID)
            {
                return Ok(elt);
            }
        }
        return NotFound("Utilisateur non trouvé.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost("~/CreateUser/{identifiant:int}/{nom}")]
    public async Task<IActionResult> AddContains(int identifiant, string nom)
    {
        try
        {
            List<Utilisateur> DataBaseContext = await UsersListe();

            Utilisateur utilisateur = new Utilisateur() { ID = identifiant, Nom = nom };
            DataBaseContext.Add(utilisateur);

            await _content.SaveChangesAsync();
            DataBaseContext = await UsersListe();
            
            return Ok("La ressource a bien été créée");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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
            if (item == null)
            {
                return NotFound($"L'utilisateur [{ID}] n'existe plus dans le contexte de base de données");
            }
            _content.Utilisateurs.Remove(item);
            //await _content.SaveChangesAsync();

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


}
