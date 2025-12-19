using Microsoft.AspNetCore.Mvc;
using NewsManagementService.Interfaces.Repositories;
using NewsManagementService.Models;
using System.Threading.Tasks;

namespace NewsManagementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsSummaryController : ControllerBase
    {
        private readonly INewsRepository _repository;

        public NewsSummaryController(INewsRepository repository)
        {
            _repository = repository;
        }


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
            return await _repository.GetAllNewsAsync();
        }

        [HttpGet]
        [Route("category/{id}")]
        public async Task<List<NewsSummary>> GetNewsByCategoryId(int categoryId) 
        { 
            return await _repository.GetNewsByCategoryId(categoryId);
        }
    }
}
