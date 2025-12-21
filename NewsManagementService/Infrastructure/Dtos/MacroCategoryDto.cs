namespace NewsManagementService.Infrastructure.DTOs;

public class MacroCategoryDto
{
    public string Name { get; set; } // Ej: "Economía"
    public List<CategoryDto> Categories { get; set; }
}