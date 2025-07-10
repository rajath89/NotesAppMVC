using Web.Models.Entities;

namespace Web.Models.Interfaces;

public interface INoteRepository
{
    Task<IEnumerable<Note>> GetByWorkspaceIdAsync(int workspaceId);
    Task<Note?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Note note);
    Task<bool> UpdateAsync(Note note);
    Task<bool> DeleteAsync(int id);
}