using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Models;

namespace NewsManagementService.Interfaces.Repositories
{
    public interface INewsRepository
    {
        public Task<List<MacroCategoryDto>> GetAllNewsDataByUserIdAsync(int userId);
        public Task SaveGeminiNewsBatchAsync(GeminiResponseDto responseData);
        public Task<NewsAppResponseDto> GetNewsDataForN8NAsync();
        public Task<NewsAppResponseDto> GetNewsDataForN8NByHourAsync(int hour);
    }
}
