using NewsManagementService.Infrastructure.DTOs;

namespace NewsManagementService.Interfaces.Repositories;

public interface IUserPreferencesReplicaRepository
{
    public Task SaveUserPreferencesAsync(UserPreferencesDto userPreferencesDto);
    public Task<NewsAppResponseDto> GetNewsDataForN8NAsync();
}