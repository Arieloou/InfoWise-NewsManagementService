using System.ComponentModel.DataAnnotations;

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
        
        [Required]
        public int ShippingHour { get; set; }
        
        public List<NewsCategory> SubscribedNewsCategories { get; set; } = [];
    }
}