using NewsManagementService.Models;

namespace NewsManagementService.Interfaces.Repositories
{
    public interface INewsCategoriesRepository
    {
        public Task<List<string>> GetAllNewsCategoriesNames();
        public Task AddNewsCategory(NewsCategory newsCategory);
    }
}
