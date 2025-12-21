using Microsoft.AspNetCore.Mvc;
using NewsManagementService.Interfaces.Repositories;

namespace NewsManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscribersController(IUserPreferencesReplicaRepository repository) : ControllerBase
    {
        [HttpPost("subscribers")]
        public async Task<IActionResult> GetSubscribers([FromBody] List<string> categoryNames)
        {
            var emails = await repository.GetEmailsByCategoryNamesAsync(categoryNames);
        
            // Example: It can return : ["juan@gmail.com", "pedro@hotmail.com"]
            return Ok(emails.Distinct()); 
        }
    }
}