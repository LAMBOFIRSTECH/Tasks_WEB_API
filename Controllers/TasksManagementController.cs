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
    _usercontext.Utilisateurs.OrderBy(u => u.Nom).ToListAsync();

    // [HttpPost]
    // public IActionResult AddContains()
    // {
    //     var tache = "";
    //     return tache;
    // }
}
