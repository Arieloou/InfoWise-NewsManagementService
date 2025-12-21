using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsManagementService.Models;
public class NewsSummary
{
    [Key]
    public int Id { get; set; }
    [Required]
    public DateTime Date { get; set; }
    public string Title { get; set; } = string.Empty;
    [Required]
    public required string Content { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public List<NewsCategory> NewsCategories { get; set; } = [];
}