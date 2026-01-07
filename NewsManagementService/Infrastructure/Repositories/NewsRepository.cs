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
        
        private async Task<List<NewsSummary>> GetFlatNewsListAsync(int userId)
        {
            return await context.NewsSummaries
                .AsNoTracking()
                .Include(n => n.NewsCategory)
                .ThenInclude(nc => nc!.MacroNewsCategory)
                .Where(n => n.NewsCategory != null && 
                            n.NewsCategory.UserPreferences!.Any(u => u.UserId == userId))
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }
        
        public async Task<List<MacroCategoryDto>> GetAllNewsDataByUserIdAsync(int userId)
        {
            var flatNewsList = await GetFlatNewsListAsync(userId);
            
            // Estructura: Macro -> Categories -> Summaries
            var hierarchicalData = flatNewsList
                .Where(n => n.NewsCategory?.MacroNewsCategory != null) 
                .GroupBy(n => n.NewsCategory!.MacroNewsCategory!.Name) // First Level: Group by Macro
                .Select(macroGroup => new MacroCategoryDto
                {
                    Name = macroGroup.Key!,
            
                    CategoryDtos = macroGroup
                        .GroupBy(n => n.NewsCategory!.Name) // Second Level: Group by Category within the Macro
                        .Select(catGroup => new CategoryDto
                        {
                            Name = catGroup.Key!,
                    
                            NewsSummaryDtos = catGroup.Select(n => new NewsSummaryDto
                            {
                                Title = n.Title,
                                Content = n.Content,
                                Date = n.Date
                            }).ToList() // Third Level: Final list of news
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
                    .FirstOrDefaultAsync(m => m.Name == macroCategoryDto.Name);

                if (macroEntity == null)
                {
                    macroEntity = new MacroNewsCategory { Name = macroCategoryDto.Name };
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
                            Title = $"Summary of {summaryDto.Title} - {today.ToShortDateString()}", 
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
    }
}
