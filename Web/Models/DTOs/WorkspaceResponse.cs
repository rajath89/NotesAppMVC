namespace Web.Models.DTOs;

public class WorkspaceResponse : ApiResponse<List<WorkspaceDto>>
{
    public List<WorkspaceDto>? Workspaces { get; set; }
}