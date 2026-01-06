namespace NewsManagementService.Infrastructure.DTOs;

public class FormatedCategoryDto
{
    public required string NewsCategoryName { get; set; }
    public NewsSummaryDto? NewsSummaryDto { get; set; }
    public List<EmailDto>? SubscribedUserEmails { get; set; }
}