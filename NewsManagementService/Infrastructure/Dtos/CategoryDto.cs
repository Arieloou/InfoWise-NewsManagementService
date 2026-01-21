namespace NewsManagementService.Infrastructure.DTOs;

public class CategoryDto
{
    public int? Id { get; set; }
    public required string Name { get; set; } // Example: "bitcoin"
    public List<NewsSummaryDto>? NewsSummaryDtos { get; set; } // The generated summary
}