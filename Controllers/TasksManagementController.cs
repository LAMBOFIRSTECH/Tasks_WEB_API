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
    private readonly DailyTasksContext _content;

    private readonly ILogger<TasksManagementController> _logger;
    public TasksManagementController(ILogger<TasksManagementController> logger, DailyTasksContext context)
    {
        _logger = logger;
        _content = context;

        _content.Database.EnsureCreated();


        // foreach (var item in utilisateurs)
        // {
        //     var searchValue = item;
        //     bool result = _content.Utilisateurs.Contains(searchValue);
        //     if (result)
        //     {
        //         return NotFound($"L'utilisateur {item.ID} est déjà présent dans le context");
        //     }


        // }

    }
    [NonAction]
    public async Task<List<Tache>> ListeTaches() 
    {
        _content.Database.EnsureCreated();
        List<Tache> taches = new List<Tache>()
        {
            new Tache(){ID="01a", Titre = "faire un audit digital",Summary="prppprp",Date=DateTime.Now},
            new Tache(){ID="01b", Titre = "demoulage",Summary="oooooooo",Date=DateTime.Now}
        };
        foreach (var item in taches)
        {
            _content.Taches.Add(item);

        }

        await _content.SaveChangesAsync();


        List<Tache> listeTaches = await _content.Taches.ToListAsync();

        return listeTaches;


    }


    [Route("~/GetTaskList")]
    [HttpGet]
    public async Task<IActionResult> GetTask()
    {

        try
        {
            var taches = await _content.Taches.ToListAsync();
            if (taches.Any())
            {
                return Ok(taches);
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

    // autre controller
}
