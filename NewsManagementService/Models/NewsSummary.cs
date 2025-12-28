using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsManagementService.Models
{
    public class NewsSummary
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required, DataType(DataType.MultilineText), MaxLength(5000)]
        public required string Content { get; set; } = string.Empty;
        [Required, MaxLength(1000)]
        public string Source { get; set; } = string.Empty;
        public List<NewsCategory> NewsCategories { get; set; } = [];
        [Required]
        public DateTime Date { get; set; }
    }
}
