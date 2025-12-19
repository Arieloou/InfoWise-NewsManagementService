using Microsoft.EntityFrameworkCore;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;

namespace NewsManagementService.Infrastructure.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _context;
        public NewsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddNewsAsync(NewsSummary news)
        {
            await _context.NewsSummaries.AddAsync(news);
        }

        public async Task<List<NewsSummary>> GetAllNewsAsync()
        {
            return await  _context.NewsSummaries
                .Include(n => n.NewsCategories)
                .ToListAsync();
        }

        public async Task<List<NewsSummary>> GetNewsByCategoryId(int id)
        {
            return await _context.NewsSummaries
                .Where(n => n.NewsCategories.Any(c => c.Id == id))
                .Include(n => n.NewsCategories)
                .ToListAsync();
        }
    }
}
