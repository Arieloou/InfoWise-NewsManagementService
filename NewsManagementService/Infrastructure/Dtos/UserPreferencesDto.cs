namespace NewsManagementService.Infrastructure.DTOs;

public class UserPreferencesDto
{
    public int UserId { get; set; }
    public string Email { get; set; } 
    public List<int> CategoryIds { get; set; }
}