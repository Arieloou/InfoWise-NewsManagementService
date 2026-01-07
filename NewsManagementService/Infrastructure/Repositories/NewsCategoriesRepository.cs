using Microsoft.EntityFrameworkCore;
using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;

namespace NewsManagementService.Infrastructure.Repositories
{
    public class NewsCategoriesRepository(ApplicationDbContext context) : INewsCategoriesRepository
    {
        public async Task AddNewsCategory(NewsCategory newsCategory)
        {
            await context.NewsCategories.AddAsync(newsCategory);
        }

        public async Task<List<NewsCategoryResponseDto>> GetAllNewsCategories()
        {
            return await context.NewsCategories
                .Select(category => new NewsCategoryResponseDto
                {
                    NewsCategoryId = category.Id,
                    NewsCategoryName = category.Name
                } )
                .ToListAsync();
        }
    }
}
