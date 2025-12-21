using Microsoft.EntityFrameworkCore;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;

namespace NewsManagementService.Infrastructure.Repositories
{
    public class NewsCategoriesRepository(ApplicationDbContext context) : INewsCategoriesRepository
    {
        public async Task AddNewsCategoryAsync(NewsCategory newsCategory)
        {
            await context.NewsCategories.AddAsync(newsCategory);
        }

        public async Task<List<NewsCategory>> GetAllNewsCategoriesAsync()
        {
            return await context.NewsCategories
                .Include(c => c.NewsSummaries)
                .ToListAsync();
        }
    }
}
