namespace NewsManagementService.Infrastructure.DTOs;

public class MacroCategoryDto
{
    public required string MacroCategoryName { get; set; }
    public List<CategoryDto>? CategoryDtos { get; set; }
}