using System.ComponentModel.DataAnnotations;

namespace NewsManagementService.Models
{
    public class MacroNewsCategory
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public required string Name { get; set; }
        public virtual ICollection<NewsCategory>? NewsCategories { get; set; }
        public virtual ICollection<NewsSummary>? NewsSummaries { get; set; }
    }
}

