using Microsoft.EntityFrameworkCore;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;

namespace NewsManagementService.Infrastructure.Repositories
{
    public class NewsCategoriesRepository(ApplicationDbContext context) : INewsCategoriesRepository
    {
        public async Task AddNewsCategory(NewsCategory newsCategory)
        {
            await context.NewsCategories.AddAsync(newsCategory);
        }

        public async Task<List<NewsCategory>> GetAllNewsCategories()
        {
            return await context.NewsCategories
                .Include(c => c.NewsSummaries)
                .ToListAsync();
        }
    }
}
