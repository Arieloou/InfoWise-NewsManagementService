using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;

namespace NewsManagementService.Application
{
    public class NewsAppService(INewsRepository newsRepository, INewsCategoriesRepository newsCategoriesRepository, IMacroNewsCategoriesRepository macroNewsCategoriesRepository,IUserPreferencesReplicaRepository userPreferencesReplicaRepository)
    {
        public async Task<List<NewsCategoryResponseDto>> GetAllNewsCategories()
        {
            return await newsCategoriesRepository.GetAllNewsCategories();
        }
        
        public async Task<List<MacroCategoryDto>> GetAllNewsDataByUserId(int userId)
        {
            return await newsRepository.GetAllNewsDataByUserIdAsync(userId);
        }
        
        public async Task SaveNewsSummaryInformation(GeminiResponseDto geminiData)
        {
            await newsRepository.SaveGeminiNewsBatchAsync(geminiData);
        }
        
        public async Task SaveUserPreferencesInformation(UserPreferencesDto userPreferencesData)
        {
            await userPreferencesReplicaRepository.SaveUserPreferencesAsync(userPreferencesData);
        }

        public async Task<NewsAppResponseDto> GetNewsDataForN8N()
        {
            return await newsRepository.GetNewsDataForN8NAsync();
        }
        
        public async Task<NewsAppResponseDto> GetNewsDataForN8NByHour(int hour)
        {
            return await newsRepository.GetNewsDataForN8NByHourAsync(hour);
        }

        public async Task<List<MacroCategoryDto>> GetMacrocategoriesWithCategories()
        {
            return await macroNewsCategoriesRepository.GetAllDtos();
        }
    }
}