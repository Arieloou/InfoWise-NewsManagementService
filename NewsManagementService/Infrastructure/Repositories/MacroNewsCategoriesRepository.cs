using Microsoft.EntityFrameworkCore;
using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;

namespace NewsManagementService.Infrastructure.Repositories;

public class MacroNewsCategoriesRepository(ApplicationDbContext context) : IMacroNewsCategoriesRepository
{
    public async Task<List<MacroNewsCategory>> GetAll()
    {
        return await context.MacroNewsCategories.Include(m => m.NewsCategories).ToListAsync();
    }

    public async Task<MacroNewsCategory?> GetById(int id)
    {
        return await context.MacroNewsCategories.FindAsync(id);
    }

    public async Task AddAsync(MacroNewsCategory macroNewsCategory)
    {
        await context.MacroNewsCategories.AddAsync(macroNewsCategory);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(MacroNewsCategory macroNewsCategory)
    {
        var macroCategoryInDb = await context.MacroNewsCategories.FindAsync(macroNewsCategory.Id);

        if (macroCategoryInDb != null)
        {
            context.MacroNewsCategories.Remove(macroCategoryInDb);
            await context.SaveChangesAsync();
        }
    }

    public async Task<List<MacroCategoryDto>> GetAllDtos()
    {
        return await context.MacroNewsCategories.Include(m => m.NewsCategories).Select(e => new MacroCategoryDto
        {
            MacroCategoryName = e.Name,
            CategoryDtos = e.NewsCategories!.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList()
        }).ToListAsync();
    }
}