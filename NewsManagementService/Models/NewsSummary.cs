using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsManagementService.Models
{
    public class NewsSummary
    {
        [Key]
        public int Id { get; set; }
        
        [Required, MaxLength(300)]
        public string Title { get; set; } = string.Empty;
        
        [Required, DataType(DataType.MultilineText), MaxLength(5000)]
        public required string Content { get; set; } = string.Empty;
        
        [Required, ForeignKey(nameof(NewsCategory))]
        public int NewsCategoryId { get; set; }
        public NewsCategory? NewsCategory { get; set; }
        
        [Required, MaxLength(1000)]
        public string Source { get; set; } = string.Empty;
        
        [Required]
        public DateTime Date { get; set; }
    }
}