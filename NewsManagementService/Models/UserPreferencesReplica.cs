using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsManagementService.Models
{
    public class UserPreferencesReplica
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required, MaxLength(100)]
        public required string Email { get; set; }
        public List<NewsCategory> NewsCategories { get; set; } = [];
    }
}

