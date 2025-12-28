using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Infrastructure.Repositories;
using NewsManagementService.Models;

namespace NewsManagementService.Application
{
    public class NewsAppService(NewsRepository newsRepository, NewsCategoriesRepository newsCategoriesRepository, UserPreferencesReplicaRepository userPreferencesReplicaRepository)
    {
        public async Task<List<NewsCategory>> GetAllNewsCategories()
        {
            return await newsCategoriesRepository.GetAllNewsCategories();
        }
        
        public async Task<List<NewsSummary>> GetAllNewsSummary()
        {
            return await newsRepository.GetAllNewsAsync();
        }

        public async Task<List<NewsSummary>> GetAllNewsSummariesByUserId(int userId)
        {
            return await newsRepository.GetNewsSummaryByUserId(userId);
        }
        
        public async Task SaveNewsSummaryInformation(GeminiRootDto geminiData)
        {
            await newsRepository.SaveGeminiNewsBatchAsync(geminiData);
        }
        
        public async Task SaveUserPreferencesInformation(UserPreferencesDto userPreferencesData)
        {
            await userPreferencesReplicaRepository.SaveUserPreferencesAsync(userPreferencesData);
        }
    }
}