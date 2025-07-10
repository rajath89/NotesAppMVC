using Web.Models.Entities;

namespace Web.Models.Interfaces;

public interface IWorkspaceRepository
{
    Task<IEnumerable<Workspace>> GetAllAsync();
    Task<Workspace?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Workspace workspace);
    Task<bool> UpdateAsync(Workspace workspace);
    Task<bool> DeleteAsync(int id);
}