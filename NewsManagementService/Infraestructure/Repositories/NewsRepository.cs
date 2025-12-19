using Microsoft.EntityFrameworkCore;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;

namespace NewsManagementService.Infraestructure.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDBContext _context;
        public NewsRepository(ApplicationDBContext context)
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
