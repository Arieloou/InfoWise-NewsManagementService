using Microsoft.EntityFrameworkCore;
using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;

namespace NewsManagementService.Infrastructure.Repositories
{
    public class NewsRepository(ApplicationDbContext context) : INewsRepository
    {
        public async Task AddNewsAsync(NewsSummary news)
        {
            await context.NewsSummaries.AddAsync(news);
        }
        
        public async Task<List<MacroCategoryDto>> GetAllNewsDataByUserIdAsync(int userId)
        {
            // Obtener todas las categorías a las que el usuario está suscrito con sus relaciones
            var subscribedCategories = await context.NewsCategories
                .AsNoTracking()
                .Include(nc => nc.MacroNewsCategory)
                .Include(nc => nc.NewsSummaries)
                .Where(nc => nc.UserPreferences!.Any(u => u.UserId == userId))
                .ToListAsync();
            
            // Estructura: Macro -> Categories -> Summaries
            var hierarchicalData = subscribedCategories
                .Where(nc => nc.MacroNewsCategory != null)
                .GroupBy(nc => nc.MacroNewsCategory!.Name) // First Level: Group by Macro
                .Select(macroGroup => new MacroCategoryDto
                {
                    MacroCategoryName = macroGroup.Key!,
            
                    CategoryDtos = macroGroup
                        .Select(category => new CategoryDto
                        {
                            Name = category.Name,
                    
                            NewsSummaryDtos = category.NewsSummaries?
                                .OrderByDescending(n => n.Date)
                                .Select(n => new NewsSummaryDto
                                {
                                    Title = n.Title,
                                    Content = n.Content,
                                    Date = n.Date
                                }).ToList() ?? new List<NewsSummaryDto>()
                        }).ToList()
                })
                .ToList();

            return hierarchicalData;
        }
        
        public async Task SaveGeminiNewsBatchAsync(GeminiResponseDto responseData)
        {
            var today = DateTime.UtcNow;

            foreach (var macroCategoryDto in responseData.MacrocategoryDtos)
            {
                var macroEntity = await context.MacroNewsCategories
                    .FirstOrDefaultAsync(m => m.Name == macroCategoryDto.MacroCategoryName);

                if (macroEntity == null)
                {
                    macroEntity = new MacroNewsCategory { Name = macroCategoryDto.MacroCategoryName };
                    await context.MacroNewsCategories.AddAsync(macroEntity);
                    await context.SaveChangesAsync();
                }

                foreach (var categoryDto in macroCategoryDto.CategoryDtos)
                {
                    var categoryEntity = await context.NewsCategories
                        .FirstOrDefaultAsync(c => c.Name == categoryDto.Name && c.MacroNewsCategoryId == macroEntity.Id);

                    if (categoryEntity == null)
                    {
                        categoryEntity = new NewsCategory 
                        { 
                            Name = categoryDto.Name, 
                            MacroNewsCategoryId = macroEntity.Id 
                        };
                        await context.NewsCategories.AddAsync(categoryEntity);
                        await context.SaveChangesAsync();
                    }

                    foreach (var summaryDto in categoryDto.NewsSummaryDtos)
                    {
                        var newsSummary = new NewsSummary
                        {
                            Content = summaryDto.Content,
                            Title = summaryDto.Title, 
                            Date = summaryDto.Date,
                            Source = "Gemini AI / N8N",
                            NewsCategoryId = categoryEntity.Id
                        };

                        await context.NewsSummaries.AddAsync(newsSummary);
                    }
                }
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
                .Where(c => c.UserPreferences.Any(u => u.ShippingHour == hour))
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
    }
}
