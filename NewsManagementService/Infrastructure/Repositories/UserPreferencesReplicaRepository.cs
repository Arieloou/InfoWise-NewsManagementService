using Microsoft.EntityFrameworkCore;
using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;

namespace NewsManagementService.Infrastructure.Repositories;

public class UserPreferencesReplicaRepository(ApplicationDbContext context) : IUserPreferencesReplicaRepository
{
    public async Task SaveUserPreferencesAsync(UserPreferencesDto userPreferencesDto)
    {
        ArgumentNullException.ThrowIfNull(userPreferencesDto);
        
        var categories = await context.NewsCategories
            .Where(c => userPreferencesDto.CategoryIds.Contains(c.Id))
            .ToListAsync();

        var existingReplica = await context.UserPreferencesReplicas
            .Include(u => u.NewsCategories) 
            .FirstOrDefaultAsync(u => u.UserId == userPreferencesDto.UserId);

        if (existingReplica != null)
        {
            existingReplica.Email = userPreferencesDto.Email;
            existingReplica.NewsCategories.Clear();
        
            foreach (var category in categories)
            {
                existingReplica.NewsCategories.Add(category);
            }
        }
        else
        {
            var newReplica = new UserPreferencesReplica
            {
                UserId = userPreferencesDto.UserId,
                Email = userPreferencesDto.Email,
                NewsCategories = categories
            };
        
            await context.UserPreferencesReplicas.AddAsync(newReplica);
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<string>> GetEmailsByCategoryNamesAsync(List<string> categoryNames)
    {
        // Select all the users where ANY of their categories has a Name included in the categoryNames list
        var emails = await context.UserPreferencesReplicas
            .Where(user => user.NewsCategories.Any(cat => categoryNames.Contains(cat.Name)))
            .Select(user => user.Email)
            .Distinct()
            .ToListAsync();

        return emails;
    }
}