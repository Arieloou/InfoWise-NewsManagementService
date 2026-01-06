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

        public async Task<List<string>> GetAllNewsCategoriesNames()
        {
            return await context.NewsCategories
                .Select(newsCategory => newsCategory.Name)
                .ToListAsync();
        }
    }
}
