using NewsManagementService.Models;

namespace NewsManagementService.Interfaces.Repositories
{
    public interface INewsCategoriesRepository
    {
        public Task<List<NewsCategory>> GetAllNewsCategoriesAsync();
        public Task AddNewsCategoryAsync(NewsCategory newsCategory);
    }
}
