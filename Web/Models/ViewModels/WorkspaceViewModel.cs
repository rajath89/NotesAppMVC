namespace Web.Models;
using System.ComponentModel.DataAnnotations;

public class WorkspaceViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Workspace name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
    public string Description { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public List<NoteViewModel> Notes { get; set; } = new();
}