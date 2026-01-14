namespace NewsManagementService.Infrastructure.DTOs;

public class UserPreferencesDto
{
    public required int UserId { get; set; }
    public required string Email { get; set; } 
    public required int ShippingHour { get; set; }
    public List<int>? CategoryIds { get; set; }
}