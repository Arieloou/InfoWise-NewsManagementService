namespace NewsManagementService.Infrastructure.DTOs;

public class CategoryDto
{
    public required string Name { get; set; } // Example: "bitcoin"
    public List<NewsSummaryDto>? NewsSummaryDtos { get; set; } // The generated summary
}