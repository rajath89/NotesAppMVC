namespace Web.Models.DTOs;

public class NoteResponse : ApiResponse<List<NoteDto>>
{
    public List<NoteDto>? Notes { get; set; }
}