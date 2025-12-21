using NewsManagementService.Models;

namespace NewsManagementService.Interfaces.Repositories;

public interface IMacroNewsCategoriesRepository
{
    public Task<List<MacroNewsCategory>> GetAll();
    public Task<MacroNewsCategory?> GetById(int id);
    public Task AddAsync(MacroNewsCategory macroNewsCategory);
    public Task DeleteAsync(MacroNewsCategory macroNewsCategory);
}