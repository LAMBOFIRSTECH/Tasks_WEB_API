using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tasks_WEB_API.Models;

namespace Tasks_WEB_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksManagementController : ControllerBase
{



    private readonly TasksManagementContext _usercontext;

    private readonly ILogger<TasksManagementController> _logger;

    public TasksManagementController(ILogger<TasksManagementController> logger, TasksManagementContext context)
    {
        _logger = logger;
        _usercontext = context;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<Utilisateur>> Get() =>
    _usercontext.Utilisateurs.OrderBy(u => u.Matricule).ToListAsync();

    // [HttpPost]
    // public IActionResult AddContains()
    // {
    //     var tache = "";
    //     return tache;
    // }

    /// <summary>
    /// On veut supprimer une tache à l'aide de son matricule
    /// </summary>
    /// <param name="Matricule"></param>
    /// <returns></returns>
    [HttpDelete("{Matricule}")]
    public async Task<IActionResult> Delete(string Matricule)
    {
        var item = await _usercontext.Taches.FindAsync(Matricule);

        if (item is null)
        {
            return NotFound();
        }

        _usercontext.Taches.Remove(item);
        await _usercontext.SaveChangesAsync();

        return NoContent();
    }
}
