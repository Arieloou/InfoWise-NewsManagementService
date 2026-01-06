namespace NewsManagementService.Infrastructure.DTOs;

public class NewsSummaryDto
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime Date { get; set; }
}