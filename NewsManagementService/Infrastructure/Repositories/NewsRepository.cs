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

        public async Task<List<NewsSummary>> GetAllNewsAsync()
        {
            return await  context.NewsSummaries
                .Include(n => n.NewsCategories)
                .ToListAsync();
        }

        public async Task<List<NewsSummary>> GetNewsByCategoryId(int id)
        {
            return await context.NewsSummaries
                .Where(n => n.NewsCategories.Any(c => c.Id == id))
                .Include(n => n.NewsCategories)
                .ToListAsync();
        }
        
        public async Task SaveGeminiNewsBatchAsync(GeminiRootDto rootData)
        {
            var today = DateTime.UtcNow;

            foreach (var macro in rootData.Macrocategories)
            {
                var macroEntity = await context.MacroNewsCategories
                    .FirstOrDefaultAsync(m => m.Name == macro.Name);

                if (macroEntity == null)
                {
                    macroEntity = new MacroNewsCategory { Name = macro.Name };
                    await context.MacroNewsCategories.AddAsync(macroEntity);
                    await context.SaveChangesAsync();
                }

                foreach (var cat in macro.Categories)
                {
                    var categoryEntity = await context.NewsCategories
                        .FirstOrDefaultAsync(c => c.Name == cat.Name && c.MacroNewsCategoryId == macroEntity.Id);

                    if (categoryEntity == null)
                    {
                        categoryEntity = new NewsCategory 
                        { 
                            Name = cat.Name, 
                            MacroNewsCategoryId = macroEntity.Id 
                        };
                        await context.NewsCategories.AddAsync(categoryEntity);
                        await context.SaveChangesAsync();
                    }
                    
                    var newsSummary = new NewsSummary
                    {
                        Content = cat.Summarie,
                        Title = $"Resumen de {cat.Name} - {today.ToShortDateString()}", 
                        Date = today,
                        Source = "Gemini AI / n8n",
                        NewsCategories = new List<NewsCategory> { categoryEntity }
                    };

                    await context.NewsSummaries.AddAsync(newsSummary);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
