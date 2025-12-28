namespace NewsManagementService.Infrastructure.DTOs;

public class NewsSummaryDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
    public List<CategoryDto> NewsCategories { get; set; }
}