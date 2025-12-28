using NewsManagementService.Models;

namespace NewsManagementService.Interfaces.Repositories
{
    public interface INewsCategoriesRepository
    {
        public Task<List<NewsCategory>> GetAllNewsCategories();
        public Task AddNewsCategory(NewsCategory newsCategory);
    }
}
