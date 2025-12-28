using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Models;

namespace NewsManagementService.Interfaces.Repositories
{
    public interface INewsRepository
    {
        public Task AddNewsAsync(NewsSummary news);
        public Task<List<NewsSummary>> GetAllNewsAsync();
        public Task<List<NewsSummary>> GetNewsSummaryByUserId(int userId);
        public Task SaveGeminiNewsBatchAsync(GeminiRootDto rootData);
    }
}
