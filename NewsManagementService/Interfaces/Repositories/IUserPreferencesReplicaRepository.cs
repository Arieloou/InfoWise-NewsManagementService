namespace NewsManagementService.Interfaces.Repositories;

public interface IUserPreferencesReplicaRepository
{
    public Task<List<string>> GetEmailsByCategoryNamesAsync(List<string> categoryNames);
}