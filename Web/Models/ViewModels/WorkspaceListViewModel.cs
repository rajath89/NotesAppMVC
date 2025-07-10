namespace Web.Models;

public class WorkspaceListViewModel
{
    public List<WorkspaceViewModel> Workspaces { get; set; } = new();
    public int? SelectedWorkspaceId { get; set; }
    public int? SelectedNoteId { get; set; }
}