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
        
	// 1. Definición de Macro Categorías
	var macroTech = new MacroNewsCategory { Name = "Tecnología" };
	var macroSports = new MacroNewsCategory { Name = "Deportes" };
	var macroEconomy = new MacroNewsCategory { Name = "Economía" };
	var macroWorld = new MacroNewsCategory { Name = "Mundo" };

	await context.MacroNewsCategories.AddRangeAsync(macroTech, macroSports, macroEconomy, macroWorld);

	// 2. Definición de Subcategorías (NewsCategory)

	// Subcategorías de Tecnología
	var catAI_ML = new NewsCategory { Name = "Inteligencia Artificial y Aprendizaje Automático", MacroNewsCategory = macroTech };
	var catCiberseguridad = new NewsCategory { Name = "Ciberseguridad y Protección de Datos", MacroNewsCategory = macroTech };
	var catDev_Arch = new NewsCategory { Name = "Desarrollo de Software y Arquitectura de Sistemas", MacroNewsCategory = macroTech };
	var catCloud = new NewsCategory { Name = "Computación en la Nube (Cloud Computing)", MacroNewsCategory = macroTech };
	var catHardware = new NewsCategory { Name = "Hardware y Arquitectura de Computadores", MacroNewsCategory = macroTech };
	var catBlockchain = new NewsCategory { Name = "Tecnologías de Registro Distribuido (Blockchain)", MacroNewsCategory = macroTech };
	var catTelecom = new NewsCategory { Name = "Telecomunicaciones y Redes 5G/6G", MacroNewsCategory = macroTech };

	// Subcategorías de Deportes
	var catFutbolInt = new NewsCategory { Name = "Fútbol Internacional", MacroNewsCategory = macroSports };
	var catBaloncesto = new NewsCategory { Name = "Baloncesto Profesional (NBA/Euroliga)", MacroNewsCategory = macroSports };
	var catTenis = new NewsCategory { Name = "Tenis de Élite (ATP/WTA)", MacroNewsCategory = macroSports };
	var catAutomovilismo = new NewsCategory { Name = "Automovilismo y Deportes de Motor", MacroNewsCategory = macroSports };
	var catCiclismo = new NewsCategory { Name = "Ciclismo de Ruta", MacroNewsCategory = macroSports };
	var catCombate = new NewsCategory { Name = "Deportes de Combate", MacroNewsCategory = macroSports };
	var catAtletismo = new NewsCategory { Name = "Atletismo y Competiciones Olímpicas", MacroNewsCategory = macroSports };

	// Subcategorías de Economía
	var catMacro_PM = new NewsCategory { Name = "Macroeconomía y Política Monetaria", MacroNewsCategory = macroEconomy };
	var catMercados = new NewsCategory { Name = "Mercados Financieros y Bursátiles", MacroNewsCategory = macroEconomy };
	var catFintech = new NewsCategory { Name = "Tecnología Financiera (Fintech)", MacroNewsCategory = macroEconomy };
	var catComercioExt = new NewsCategory { Name = "Comercio Exterior y Tratados Internacionales", MacroNewsCategory = macroEconomy };
	var catCripto = new NewsCategory { Name = "Criptoeconomía y Activos Digitales", MacroNewsCategory = macroEconomy };
	var catInmobiliario = new NewsCategory { Name = "Mercado Inmobiliario (Real Estate)", MacroNewsCategory = macroEconomy };
	var catEnergia_Sust = new NewsCategory { Name = "Gestión de la Energía y Sostenibilidad Económica", MacroNewsCategory = macroEconomy };

	// Subcategorías de Mundo
	var catGeopolitica = new NewsCategory { Name = "Geopolítica y Relaciones Internacionales", MacroNewsCategory = macroWorld };
	var catConflictos = new NewsCategory { Name = "Conflictos Transfronterizos y Diplomacia", MacroNewsCategory = macroWorld };
	var catAmbiente = new NewsCategory { Name = "Medio Ambiente y Cambio Climático", MacroNewsCategory = macroWorld };
	var catDerechos = new NewsCategory { Name = "Derechos Humanos y Legislación Internacional", MacroNewsCategory = macroWorld };
	var catSaludGlobal = new NewsCategory { Name = "Salud Pública Global", MacroNewsCategory = macroWorld };
	var catMigracion = new NewsCategory { Name = "Dinámicas Migratorias y Demografía", MacroNewsCategory = macroWorld };
	var catMultilaterales = new NewsCategory { Name = "Organizaciones Multilaterales", MacroNewsCategory = macroWorld };


	// 3. Agregar todas las categorías al contexto
	var allNewsCategories = new NewsCategory[]
	{
	    // Tecnología
	    catAI_ML, catCiberseguridad, catDev_Arch, catCloud, catHardware, catBlockchain, catTelecom,
	    // Deportes
	    catFutbolInt, catBaloncesto, catTenis, catAutomovilismo, catCiclismo, catCombate, catAtletismo,
	    // Economía
	    catMacro_PM, catMercados, catFintech, catComercioExt, catCripto, catInmobiliario, catEnergia_Sust,
	    // Mundo
	    catGeopolitica, catConflictos, catAmbiente, catDerechos, catSaludGlobal, catMigracion, catMultilaterales
	};

	await context.NewsCategories.AddRangeAsync(allNewsCategories);
            
        var newsList = new List<NewsSummary>
            {
                new NewsSummary
                {
                    Title = "Avances en LLMs en 2025",
                    Content = "Los modelos de lenguaje siguen evolucionando con nuevas arquitecturas...",
                    Source = "TechCrunch",
                    Date = DateTime.UtcNow.AddDays(-2),
                    NewsCategory = catAI_ML
                },
                new NewsSummary
                {
                    Title = ".NET 9 Preview lanzado",
                    Content = "Microsoft ha anunciado las nuevas características de rendimiento...",
                    Source = "Microsoft Blog",
                    Date = DateTime.UtcNow.AddDays(-1),
                    NewsCategory = catDev_Arch
                },
                new NewsSummary
                {
                    Title = "Final del Mundial de Clubes",
                    Content = "Un partido emocionante define al nuevo campeón mundial...",
                    Source = "ESPN",
                    Date = DateTime.UtcNow.AddHours(-5),
                    NewsCategory = catFutbolInt
                }
            };

            context.NewsSummaries.AddRange(newsList);
            
        await context.SaveChangesAsync();
    }
}
