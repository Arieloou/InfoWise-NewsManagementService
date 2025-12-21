using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Interfaces.Repositories;

namespace NewsManagementService;

public class NewsAppService(INewsRepository newsRepository)
{
    public async Task SaveNewsSummaryInformation(GeminiRootDto geminiData)
    {
        await newsRepository.SaveGeminiNewsBatchAsync(geminiData);
    }
}