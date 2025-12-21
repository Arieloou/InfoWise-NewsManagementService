using Microsoft.EntityFrameworkCore;
using NewsManagementService.Interfaces.Repositories;

namespace NewsManagementService.Infrastructure.Repositories;

public class UserPreferencesReplicaRepository(ApplicationDbContext context) : IUserPreferencesReplicaRepository
{
    public async Task<List<string>> GetEmailsByCategoryNamesAsync(List<string> categoryNames)
    {
        var categoryIds = await context.NewsCategories
            .Where(c => categoryNames.Contains(c.Name))
            .Select(c => c.Id)
            .ToListAsync();
        
        var emails = await context.UserPreferencesReplicas
            .Where(s => categoryIds.Contains(s.CategoryId))
            .Select(s => s.Email)
            .Distinct()
            .ToListAsync();

        return emails;
    }
}