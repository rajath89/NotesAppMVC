using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Models.Entities;
using Web.Models.Interfaces;

namespace Web.Controllers;

[Authorize]
public class NotesController : Controller
{
    private readonly INoteRepository _noteRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly ILogger<NotesController> _logger;

    public NotesController(INoteRepository noteRepository, IWorkspaceRepository workspaceRepository, ILogger<NotesController> logger)
    {
        _noteRepository = noteRepository;
        _workspaceRepository = workspaceRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int? workspaceId)
    {
        IEnumerable<Note> notes;
        
        if (workspaceId.HasValue)
        {
            notes = await _noteRepository.GetByWorkspaceIdAsync(workspaceId.Value);
        }
        else
        {
            // Get all workspaces and their notes
            var workspaces = await _workspaceRepository.GetAllAsync();
            notes = workspaces.SelectMany(w => w.Notes);
        }

        var workspacesList = await _workspaceRepository.GetAllAsync();
        var viewModel = new WorkspaceListViewModel
        {
            Workspaces = workspacesList.Select(w => new WorkspaceViewModel
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description,
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
            }).ToList(),
            SelectedWorkspaceId = workspaceId
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        if (note == null)
        {
            return NotFound();
        }

        var workspace = await _workspaceRepository.GetByIdAsync(note.WorkspaceId);
        ViewBag.Workspaces = await _workspaceRepository.GetAllAsync();
        var viewModel = new NoteViewModel
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            WorkspaceId = note.WorkspaceId,
            WorkspaceName = workspace?.Name ?? "Unknown",
            CreatedAt = note.CreatedAt,
            ModifiedAt = note.ModifiedAt
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Create(int? workspaceId)
    {
        var workspaces = await _workspaceRepository.GetAllAsync();
        var viewModel = new NoteCreateViewModel
        {
            WorkspaceId = workspaceId ?? 0,
            Workspaces = workspaces.Select(w => new WorkspaceViewModel
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NoteCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var workspaces = await _workspaceRepository.GetAllAsync();
            model.Workspaces = workspaces.Select(w => new WorkspaceViewModel
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description
            }).ToList();
            return View(model);
        }

        var note = new Note
        {
            Title = model.Title,
            Content = model.Content,
            WorkspaceId = model.WorkspaceId,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _noteRepository.CreateAsync(note);
        if (result)
        {
            TempData["Success"] = "Note created successfully.";
            return RedirectToAction("Index", new { workspaceId = model.WorkspaceId });
        }

        ModelState.AddModelError(string.Empty, "Failed to create note. Please try again.");
        var workspacesList = await _workspaceRepository.GetAllAsync();
        model.Workspaces = workspacesList.Select(w => new WorkspaceViewModel
        {
            Id = w.Id,
            Name = w.Name,
            Description = w.Description
        }).ToList();
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        if (note == null)
        {
            return NotFound();
        }

        var workspaces = await _workspaceRepository.GetAllAsync();
        var viewModel = new NoteEditViewModel
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            WorkspaceId = note.WorkspaceId,
            Workspaces = workspaces.Select(w => new WorkspaceViewModel
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NoteEditViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            var workspaces = await _workspaceRepository.GetAllAsync();
            model.Workspaces = workspaces.Select(w => new WorkspaceViewModel
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description
            }).ToList();
            return View(model);
        }

        var existingNote = await _noteRepository.GetByIdAsync(id);
        if (existingNote == null)
        {
            return NotFound();
        }

        var note = new Note
        {
            Id = model.Id,
            Title = model.Title,
            Content = model.Content,
            WorkspaceId = model.WorkspaceId,
            CreatedAt = existingNote.CreatedAt,
            ModifiedAt = DateTime.UtcNow
        };

        var result = await _noteRepository.UpdateAsync(note);
        if (result)
        {
            TempData["Success"] = "Note updated successfully.";
            return RedirectToAction("Details", new { id = model.Id });
        }

        ModelState.AddModelError(string.Empty, "Failed to update note. Please try again.");
        var workspacesList = await _workspaceRepository.GetAllAsync();
        model.Workspaces = workspacesList.Select(w => new WorkspaceViewModel
        {
            Id = w.Id,
            Name = w.Name,
            Description = w.Description
        }).ToList();
        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        if (note == null)
        {
            return NotFound();
        }

        var workspace = await _workspaceRepository.GetByIdAsync(note.WorkspaceId);
        var viewModel = new NoteViewModel
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            WorkspaceId = note.WorkspaceId,
            WorkspaceName = workspace?.Name ?? "Unknown",
            CreatedAt = note.CreatedAt,
            ModifiedAt = note.ModifiedAt
        };

        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        var result = await _noteRepository.DeleteAsync(id);
        
        if (result)
        {
            TempData["Success"] = "Note deleted successfully.";
            return RedirectToAction("Index", new { workspaceId = note?.WorkspaceId });
        }

        TempData["Error"] = "Failed to delete note.";
        return RedirectToAction("Index");
    }
}