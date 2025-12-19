using Microsoft.EntityFrameworkCore;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;

namespace NewsManagementService.Infrastructure.Repositories
{
    public class NewsCategoriesRepository : INewsCategoriesRepository
    {
        private readonly ApplicationDbContext _context;
        public NewsCategoriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddNewsCategoryAsync(NewsCategory newsCategory)
        {
            await _context.NewsCategories.AddAsync(newsCategory);
        }

        public async Task<List<NewsCategory>> GetAllNewsCategoriesAsync()
        {
            return await _context.NewsCategories
                .Include(c => c.NewsSummaries)
                .ToListAsync();
        }
    }
}
