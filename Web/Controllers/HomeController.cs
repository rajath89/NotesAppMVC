using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Models.Interfaces;

namespace Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly INoteRepository _noteRepository;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IWorkspaceRepository workspaceRepository, INoteRepository noteRepository,
        ILogger<HomeController> logger)
    {
        _workspaceRepository = workspaceRepository;
        _noteRepository = noteRepository;
        _logger = logger;
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }
    
    public async Task<IActionResult> Index()
    {
        var workspaces = await _workspaceRepository.GetAllAsync();
        var viewModel = new WorkspaceListViewModel
        {
            Workspaces = workspaces.Select(w => new WorkspaceViewModel
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description,
                UserId = w.UserId,
                CreatedAt = w.CreatedAt,
                ModifiedAt = w.ModifiedAt,
                Notes = w.Notes.Select(n => new NoteViewModel
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    WorkspaceId = n.WorkspaceId,
                    WorkspaceName = w.Name,
                    CreatedAt = n.CreatedAt,
                    ModifiedAt = n.ModifiedAt
                }).ToList()
            }).ToList()
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}