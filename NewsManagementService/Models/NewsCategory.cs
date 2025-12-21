using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsManagementService.Models
{
    public class NewsCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; } = string.Empty;
        [ForeignKey(nameof(MacroNewsCategory))]
        public required int MacroNewsCategoryId { get; set; }
        public List<NewsSummary> NewsSummaries { get; } = [];
    }
}
