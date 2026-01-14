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
            .Include(u => u.SubscribedNewsCategories) 
            .FirstOrDefaultAsync(u => u.UserId == userPreferencesDto.UserId);

        if (existingReplica != null)
        {
            existingReplica.Email = userPreferencesDto.Email;
            existingReplica.SubscribedNewsCategories.Clear();
            foreach (var category in categories)
            {
                existingReplica.SubscribedNewsCategories.Add(category);
            }
        }
        else
        {
            var newReplica = new UserPreferencesReplica
            {
                UserId = userPreferencesDto.UserId,
                Email = userPreferencesDto.Email,
                ShippingHour = userPreferencesDto.ShippingHour,
                SubscribedNewsCategories = categories
            };
            await context.UserPreferencesReplicas.AddAsync(newReplica);
        }

        await context.SaveChangesAsync();
    }

    public async Task<NewsAppResponseDto> GetNewsDataForN8NAsync()
    {
        var categoryDtos = await context.NewsCategories
            .AsNoTracking()
            .Where(c => c.NewsSummaries.Any()) 
            .Select(category => new FormatedCategoryDto
            {
                NewsCategoryName = category.Name,

                NewsSummaryDto = category.NewsSummaries
                    .OrderByDescending(s => s.Date)
                    .Select(s => new NewsSummaryDto
                    {
                        Title = s.Title,
                        Content = s.Content,
                        Date = s.Date
                    })
                    .FirstOrDefault(), 

                SubscribedUserEmails = category.UserPreferences
                    .Select(u => new EmailDto 
                    { 
                        Email = u.Email 
                    })
                    .ToList()
            })      
            .ToListAsync();

        return new NewsAppResponseDto
        {
            NewsCategoryDtos = categoryDtos
        };
    }

    public async Task<NewsAppResponseDto> GetNewsDataForN8NByHourAsync(int hour)
    {
        var categoryDtos = await context.NewsCategories
            .AsNoTracking()
            .Where(c => c.NewsSummaries.Any()) 
            .Select(category => new FormatedCategoryDto
            {
                NewsCategoryName = category.Name,

                NewsSummaryDto = category.NewsSummaries
                    .OrderByDescending(s => s.Date)
                    .Select(s => new NewsSummaryDto
                    {
                        Title = s.Title,
                        Content = s.Content,
                        Date = s.Date
                    })
                    .FirstOrDefault(), 

                SubscribedUserEmails = category.UserPreferences
                    .Where(up => up.ShippingHour == hour)
                    .Select(u => new EmailDto 
                    { 
                        Email = u.Email 
                    })
                    .ToList()
            })      
            .ToListAsync();

        return new NewsAppResponseDto
        {
            NewsCategoryDtos = categoryDtos
        };
    }
}