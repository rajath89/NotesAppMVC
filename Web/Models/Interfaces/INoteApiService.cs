using Web.Models.DTOs;

namespace Web.Models.Interfaces;

public interface INotesApiService
{
    Task<List<WorkspaceDto>> GetWorkspacesAsync();
    Task<WorkspaceDto?> GetWorkspaceAsync(int id);
    Task<bool> CreateWorkspaceAsync(WorkspaceDto workspace);
    Task<bool> UpdateWorkspaceAsync(int id, WorkspaceDto workspace);
    Task<bool> DeleteWorkspaceAsync(int id);
        
    Task<List<NoteDto>> GetWorkspaceNotesAsync(int workspaceId);
    Task<NoteDto?> GetNoteAsync(int id);
    Task<bool> CreateNoteAsync(NoteDto note);
    Task<bool> UpdateNoteAsync(int id, NoteDto note);
    Task<bool> DeleteNoteAsync(int id);
}