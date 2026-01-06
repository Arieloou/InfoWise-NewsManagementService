namespace NewsManagementService.Infrastructure.DTOs;

public class MacroCategoryDto
{
    public required string Name { get; set; }
    public List<CategoryDto>? CategoryDtos { get; set; }
}