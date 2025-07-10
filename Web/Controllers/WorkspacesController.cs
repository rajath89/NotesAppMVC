using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Models.Entities;
using Web.Models.Interfaces;

namespace Web.Controllers;

[Authorize]
public class WorkspacesController : Controller
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly ILogger<WorkspacesController> _logger;

    public WorkspacesController(IWorkspaceRepository workspaceRepository, ILogger<WorkspacesController> logger)
    {
        _workspaceRepository = workspaceRepository;
        _logger = logger;
    }

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
                    CreatedAt = n.CreatedAt,
                    ModifiedAt = n.ModifiedAt
                }).ToList()
            }).ToList()
        };
        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null)
        {
            return NotFound();
        }

        var viewModel = new WorkspaceViewModel
        {
            Id = workspace.Id,
            Name = workspace.Name,
            Description = workspace.Description,
            UserId = workspace.UserId,
            CreatedAt = workspace.CreatedAt,
            ModifiedAt = workspace.ModifiedAt,
            Notes = workspace.Notes.Select(n => new NoteViewModel
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                WorkspaceId = n.WorkspaceId,
                WorkspaceName = workspace.Name,
                CreatedAt = n.CreatedAt,
                ModifiedAt = n.ModifiedAt
            }).ToList()
        };

        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(WorkspaceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var workspace = new Workspace
        {
            Name = model.Name,
            Description = model.Description,
            UserId = User.Identity?.Name ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _workspaceRepository.CreateAsync(workspace);
        if (result)
        {
            TempData["Success"] = "Workspace created successfully.";
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to create workspace. Please try again.");
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null)
        {
            return NotFound();
        }

        var viewModel = new WorkspaceViewModel
        {
            Id = workspace.Id,
            Name = workspace.Name,
            Description = workspace.Description,
            UserId = workspace.UserId,
            CreatedAt = workspace.CreatedAt,
            ModifiedAt = workspace.ModifiedAt
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, WorkspaceViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var workspace = new Workspace
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            UserId = model.UserId,
            CreatedAt = model.CreatedAt,
            ModifiedAt = DateTime.UtcNow
        };

        var result = await _workspaceRepository.UpdateAsync(workspace);
        if (result)
        {
            TempData["Success"] = "Workspace updated successfully.";
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to update workspace. Please try again.");
        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null)
        {
            return NotFound();
        }

        var viewModel = new WorkspaceViewModel
        {
            Id = workspace.Id,
            Name = workspace.Name,
            Description = workspace.Description,
            UserId = workspace.UserId,
            CreatedAt = workspace.CreatedAt,
            ModifiedAt = workspace.ModifiedAt
        };

        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _workspaceRepository.DeleteAsync(id);
        if (result)
        {
            TempData["Success"] = "Workspace deleted successfully.";
        }
        else
        {
            TempData["Error"] = "Failed to delete workspace.";
        }

        return RedirectToAction("Index");
    }
}