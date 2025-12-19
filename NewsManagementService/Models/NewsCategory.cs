using System.ComponentModel.DataAnnotations;

namespace NewsManagementService.Models
{
    public class NewsCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<NewsSummary> NewsSummaries { get; } = [];
    }
}
