namespace NewsManagementService.Infrastructure.DTOs;

public class NewsAppResponseDto
{
    public required List<FormatedCategoryDto> NewsCategoryDtos { get; set; }
}