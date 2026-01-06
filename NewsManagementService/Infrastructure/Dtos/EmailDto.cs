using System.ComponentModel.DataAnnotations;

namespace NewsManagementService.Infrastructure.DTOs;

public class EmailDto
{
    [EmailAddress]
    public required string Email { get; set; }
}