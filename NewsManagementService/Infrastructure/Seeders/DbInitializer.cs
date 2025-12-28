using Microsoft.EntityFrameworkCore;
using NewsManagementService.Models;

namespace NewsManagementService.Infrastructure.Seeders;

public class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Check if data already exists to avoid duplication
        if (await context.MacroNewsCategories.AnyAsync())
        {
            return; // The database has already been populated
        }

        var macros = new List<MacroNewsCategory>
        {
            new MacroNewsCategory
            {
                Name = "Tecnología",
                NewsCategories = new List<NewsCategory>
                {
                    new NewsCategory { Name = "Inteligencia Artificial" },
                    new NewsCategory { Name = "Ciberseguridad" },
                    new NewsCategory { Name = "Desarrollo de Software" },
                    new NewsCategory { Name = "Gadgets" }
                }
            },
            new MacroNewsCategory
            {
                Name = "Economía",
                NewsCategories = new List<NewsCategory>
                {
                    new NewsCategory { Name = "Criptomonedas" },
                    new NewsCategory { Name = "Mercados" },
                    new NewsCategory { Name = "Startups" }
                }
            },
            new MacroNewsCategory
            {
                Name = "Deportes",
                NewsCategories = new List<NewsCategory>
                {
                    new NewsCategory { Name = "Fútbol" },
                    new NewsCategory { Name = "Fórmula 1" },
                    new NewsCategory { Name = "Tenis" }
                }
            },
            new MacroNewsCategory
            {
                Name = "Mundo",
                NewsCategories = new List<NewsCategory>
                {
                    new NewsCategory { Name = "Política" },
                    new NewsCategory { Name = "Conflictos" },
                    new NewsCategory { Name = "Medio Ambiente" }
                }
            }
        };

        await context.MacroNewsCategories.AddRangeAsync(macros);
        await context.SaveChangesAsync();
    }
}