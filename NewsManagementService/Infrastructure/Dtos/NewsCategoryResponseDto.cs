using NewsManagementService.Models;

namespace NewsManagementService.Infrastructure.DTOs;

public class NewsCategoryResponseDto
{
    public required int NewsCategoryId { get; set; }
    public required string NewsCategoryName { get; set; }
}