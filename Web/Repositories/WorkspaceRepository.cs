using Web.Models.DTOs;
using Web.Models.Entities;
using Web.Models.Interfaces;

namespace Web.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly INotesApiService _apiService;
        private readonly ILogger<WorkspaceRepository> _logger;

        public WorkspaceRepository(INotesApiService apiService, ILogger<WorkspaceRepository> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IEnumerable<Workspace>> GetAllAsync()
        {
            try
            {
                var workspaces = await _apiService.GetWorkspacesAsync();
                return workspaces.Select(MapToEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all workspaces");
                return Enumerable.Empty<Workspace>();
            }
        }

        public async Task<Workspace?> GetByIdAsync(int id)
        {
            try
            {
                var workspace = await _apiService.GetWorkspaceAsync(id);
                return workspace != null ? MapToEntity(workspace) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workspace with id {WorkspaceId}", id);
                return null;
            }
        }

        public async Task<bool> CreateAsync(Workspace workspace)
        {
            try
            {
                var dto = MapToDto(workspace);
                return await _apiService.CreateWorkspaceAsync(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating workspace");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Workspace workspace)
        {
            try
            {
                var dto = MapToDto(workspace);
                return await _apiService.UpdateWorkspaceAsync(workspace.Id, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workspace with id {WorkspaceId}", workspace.Id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _apiService.DeleteWorkspaceAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting workspace with id {WorkspaceId}", id);
                return false;
            }
        }

        private static Workspace MapToEntity(WorkspaceDto dto)
        {
            return new Workspace
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                UserId = dto.UserId,
                CreatedAt = dto.CreatedAt,
                ModifiedAt = dto.ModifiedAt,
                Notes = dto.Notes.Select(n => new Note
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    WorkspaceId = n.WorkspaceId,
                    CreatedAt = n.CreatedAt,
                    ModifiedAt = n.ModifiedAt
                }).ToList()
            };
        }

        private static WorkspaceDto MapToDto(Workspace entity)
        {
            return new WorkspaceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                UserId = entity.UserId,
                CreatedAt = entity.CreatedAt,
                ModifiedAt = entity.ModifiedAt
            };
        }
    }
