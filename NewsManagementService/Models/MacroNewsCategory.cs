using System.ComponentModel.DataAnnotations;

namespace NewsManagementService.Models;

public class MacroNewsCategory
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
}