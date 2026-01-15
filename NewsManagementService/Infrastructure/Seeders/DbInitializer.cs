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
        
        var macroTech = new MacroNewsCategory { Name = "Tecnología" };
        var macroSports = new MacroNewsCategory { Name = "Deportes" };
        var macroEconomy = new MacroNewsCategory { Name = "Economía" };
        var macroWorld = new MacroNewsCategory { Name = "Mundo" };
        
        await context.MacroNewsCategories.AddRangeAsync(macroTech, macroSports, macroEconomy, macroWorld);
        
        var catAI = new NewsCategory { Name = "Inteligencia Artificial", MacroNewsCategory = macroTech };
        var catCiber = new NewsCategory { Name = "Ciberseguridad", MacroNewsCategory = macroTech };
        var catGadget = new NewsCategory { Name = "Gadgets", MacroNewsCategory = macroTech };
        var catDev = new NewsCategory { Name = "Desarrollo de Software", MacroNewsCategory = macroTech };
        
        var catFootball = new NewsCategory { Name = "Fútbol", MacroNewsCategory = macroSports };
        var catPolitics = new NewsCategory { Name = "Geopolítica", MacroNewsCategory = macroWorld };
        
        await context.NewsCategories.AddRangeAsync(catAI, catDev, catCiber, catGadget, catFootball, catPolitics);
        
        var newsList = new List<NewsSummary>
            {
                new NewsSummary
                {
                    Title = "Avances en LLMs en 2025",
                    Content = "Los modelos de lenguaje siguen evolucionando con nuevas arquitecturas...",
                    Source = "TechCrunch",
                    Date = DateTime.UtcNow.AddDays(-2),
                    NewsCategory = catAI
                },
                new NewsSummary
                {
                    Title = ".NET 9 Preview lanzado",
                    Content = "Microsoft ha anunciado las nuevas características de rendimiento...",
                    Source = "Microsoft Blog",
                    Date = DateTime.UtcNow.AddDays(-1),
                    NewsCategory = catDev
                },
                new NewsSummary
                {
                    Title = "Final del Mundial de Clubes",
                    Content = "Un partido emocionante define al nuevo campeón mundial...",
                    Source = "ESPN",
                    Date = DateTime.UtcNow.AddHours(-5),
                    NewsCategory = catFootball
                }
            };

            context.NewsSummaries.AddRange(newsList);
            
        await context.SaveChangesAsync();
    }
}