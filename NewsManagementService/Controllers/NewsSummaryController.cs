using Microsoft.AspNetCore.Mvc;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;
using System.Threading.Tasks;

namespace NewsManagementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsSummaryController(INewsRepository repository) : ControllerBase
    {
        [HttpGet]
        [Route("health")]
        public IActionResult HealthCheck()
        {
            return Ok("NewsSummary Service is running.");
        }
        
        [HttpGet]
        [Route("all")]
        public async Task<List<NewsSummary>> GetAllNewsSummaries()
        {
            return await repository.GetAllNewsAsync();
        }   

        [HttpGet]
        [Route("category/{id}")]
        public async Task<List<NewsSummary>> GetNewsByCategoryId(int categoryId) 
        {
            return await repository.GetNewsByCategoryId(categoryId);
        }
    }
}
