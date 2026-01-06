using System.ComponentModel.DataAnnotations;

namespace NewsManagementService.Models
{
    public sealed class MacroNewsCategory
    {
        [Key]
        public int Id { get; set; }
        
        [Required, MaxLength(50)]
        public required string Name { get; set; }
        
        public ICollection<NewsCategory>? NewsCategories { get; set; }
    }
}

