using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Models;

namespace NewsManagementService.Interfaces.Repositories
{
    public interface INewsRepository
    {
        public Task AddNewsAsync(NewsSummary news);
        public Task<List<MacroCategoryDto>> GetAllNewsDataByUserIdAsync(int userId);
        public Task SaveGeminiNewsBatchAsync(GeminiResponseDto responseData);
    }
}
