using Web.Models.DTOs;
using Web.Models.Entities;
using Web.Models.Interfaces;

namespace Web.Repositories;

public class NoteRepository : INoteRepository
    {
        private readonly INotesApiService _apiService;
        private readonly ILogger<NoteRepository> _logger;

        public NoteRepository(INotesApiService apiService, ILogger<NoteRepository> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IEnumerable<Note>> GetByWorkspaceIdAsync(int workspaceId)
        {
            try
            {
                var notes = await _apiService.GetWorkspaceNotesAsync(workspaceId);
                return notes.Select(MapToEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notes for workspace {WorkspaceId}", workspaceId);
                return Enumerable.Empty<Note>();
            }
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            try
            {
                var note = await _apiService.GetNoteAsync(id);
                return note != null ? MapToEntity(note) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting note with id {NoteId}", id);
                return null;
            }
        }

        public async Task<bool> CreateAsync(Note note)
        {
            try
            {
                var dto = MapToDto(note);
                return await _apiService.CreateNoteAsync(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating note");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Note note)
        {
            try
            {
                var dto = MapToDto(note);
                return await _apiService.UpdateNoteAsync(note.Id, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating note with id {NoteId}", note.Id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _apiService.DeleteNoteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting note with id {NoteId}", id);
                return false;
            }
        }

        private static Note MapToEntity(NoteDto dto)
        {
            return new Note
            {
                Id = dto.Id,
                Title = dto.Title,
                Content = dto.Content,
                WorkspaceId = dto.WorkspaceId,
                CreatedAt = dto.CreatedAt,
                ModifiedAt = dto.ModifiedAt
            };
        }

        private static NoteDto MapToDto(Note entity)
        {
            return new NoteDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                WorkspaceId = entity.WorkspaceId,
                CreatedAt = entity.CreatedAt,
                ModifiedAt = entity.ModifiedAt
            };
        }
    }