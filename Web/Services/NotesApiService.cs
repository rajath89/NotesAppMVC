using System.Text;
using System.Text.Json;
using Web.Models.DTOs;
using Web.Models.Interfaces;

namespace Web.Services;

public class NotesApiService : INotesApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NotesApiService> _logger;

        public NotesApiService(HttpClient httpClient, ILogger<NotesApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<WorkspaceDto>> GetWorkspacesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:5011/NotesApp/api/v1/Workspaces/GetWorkspaces");
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var workspaceResponse = JsonSerializer.Deserialize<WorkspaceResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return workspaceResponse?.Status == 0 ? workspaceResponse.Workspaces ?? new List<WorkspaceDto>() : new List<WorkspaceDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workspaces");
                return new List<WorkspaceDto>();
            }
        }

        public async Task<WorkspaceDto?> GetWorkspaceAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5011/NotesApp/api/v1/Workspaces/GetWorkspace?id={id}");
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var workspaceResponse = JsonSerializer.Deserialize<WorkspaceResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return workspaceResponse?.Status == 0 ? workspaceResponse.Workspaces?.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workspace {Id}", id);
                return null;
            }
        }

        public async Task<bool> CreateWorkspaceAsync(WorkspaceDto workspace)
        {
            try
            {
                var json = JsonSerializer.Serialize(workspace);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("/NotesApp/api/v1/Workspaces/CreateWorkspace", content);
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return apiResponse?.Status == 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating workspace");
                return false;
            }
        }

        public async Task<bool> UpdateWorkspaceAsync(int id, WorkspaceDto workspace)
        {
            try
            {
                var json = JsonSerializer.Serialize(workspace);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync($"/NotesApp/api/v1/Workspaces/UpdateWorkspace?id={id}", content);
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return apiResponse?.Status == 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workspace");
                return false;
            }
        }

        public async Task<bool> DeleteWorkspaceAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/NotesApp/api/v1/Workspaces/DeleteWorkspace?id={id}");
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return apiResponse?.Status == 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting workspace");
                return false;
            }
        }

        public async Task<List<NoteDto>> GetWorkspaceNotesAsync(int workspaceId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/NotesApp/api/v1/Notes/GetWorkspaceNotes?workspaceId={workspaceId}");
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var noteResponse = JsonSerializer.Deserialize<NoteResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return noteResponse?.Status == 0 ? noteResponse.Notes ?? new List<NoteDto>() : new List<NoteDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workspace notes");
                return new List<NoteDto>();
            }
        }

        public async Task<NoteDto?> GetNoteAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5011/NotesApp/api/v1/Notes/GetNote?id={id}");
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var noteResponse = JsonSerializer.Deserialize<NoteResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return noteResponse?.Status == 0 ? noteResponse.Notes?.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting note {Id}", id);
                return null;
            }
        }

        public async Task<bool> CreateNoteAsync(NoteDto note)
        {
            try
            {
                var json = JsonSerializer.Serialize(note);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("/NotesApp/api/v1/Notes/CreateNote", content);
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return apiResponse?.Status == 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating note");
                return false;
            }
        }

        public async Task<bool> UpdateNoteAsync(int id, NoteDto note)
        {
            try
            {
                var json = JsonSerializer.Serialize(note);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync($"/NotesApp/api/v1/Notes/UpdateNote?id={id}", content);
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return apiResponse?.Status == 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating note");
                return false;
            }
        }

        public async Task<bool> DeleteNoteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/NotesApp/api/v1/Notes/DeleteNote?id={id}");
                response.EnsureSuccessStatusCode();
                
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return apiResponse?.Status == 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting note");
                return false;
            }
        }
    }
