using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class NoteEditViewModel
{
    public int Id { get; set; }
        
    [Required(ErrorMessage = "Note title is required")]
    [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
    public string Title { get; set; } = string.Empty;
        
    public string Content { get; set; } = string.Empty;
        
    [Required(ErrorMessage = "Workspace is required")]
    public int WorkspaceId { get; set; }
        
    public List<WorkspaceViewModel> Workspaces { get; set; } = new();
}