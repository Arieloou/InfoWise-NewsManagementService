using Microsoft.AspNetCore.Mvc;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;
using System.Threading.Tasks;
using NewsManagementService.Application;

namespace NewsManagementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsSummaryController(NewsAppService service) : ControllerBase
    {
        [HttpGet]
        [Route("health")]
        public IActionResult HealthCheck()
        {
            return Ok("NewsSummary Service is running.");
        }

        [HttpGet]
        [Route("news-summary/{userId}")]
        public async Task<List<NewsSummary>> GetNewsSummaryByUserId(int userId) 
        {
            return await service.GetAllNewsSummariesByUserId(userId);
        }
    }
}
