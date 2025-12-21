using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Models;

namespace NewsManagementService.Interfaces.Repositories
{
    public interface INewsRepository
    {
        public Task<List<NewsSummary>> GetAllNewsAsync();
        public Task<List<NewsSummary>> GetNewsByCategoryId(int id);
        public Task AddNewsAsync(NewsSummary news);
        public Task SaveGeminiNewsBatchAsync(GeminiRootDto rootData);
    }
}
