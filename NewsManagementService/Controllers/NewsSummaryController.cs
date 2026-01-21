using Microsoft.AspNetCore.Mvc;
using NewsManagementService.Application;
using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Models;

namespace NewsManagementService.Controllers
{
    [ApiController]
    [Route("news-management")]
    public class NewsSummaryController(NewsAppService service, ILogger<NewsSummaryController> logger) : ControllerBase
    {
        [HttpGet]
        [Route("health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Healthy", message = "NewsSummary Service is running." });
        }

        [HttpGet]
        [Route("macrocategories-with-categories/all")]
        public async Task<ActionResult<List<MacroNewsCategory>>> GetAllMacrocategories()
        {
            try
            {
                var response = await service.GetMacrocategoriesWithCategories();
                return Ok(response);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error retrieving news categories names.");
                
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    error = "An error occurred while processing your request.",
                });
            }
        }

        [HttpGet]
        [Route("categories/all")]
        public async Task<ActionResult<List<NewsCategoryResponseDto>>> GetAllCategories()
        {
            try
            {
                var response = await service.GetAllNewsCategories();
                return Ok(response);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error retrieving news categories names.");
                
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    error = "An error occurred while processing your request.",
                });
            }
        }
        
        [HttpGet]
        [Route("summaries/user/{userId}")]
        public async Task<ActionResult<List<MacroCategoryDto>>> GetNewsDataByUserId(int userId) 
        {
            if (userId <= 0)
            {
                logger.LogWarning("Invalid user ID attempt: {UserId}", userId);
                return BadRequest(new { error = "User ID must be a positive integer." });
            }

            try
            {
                // Note: We return 200 OK even if the list is empty ([]),
                // since it's a valid response (the user exists, but has no news).
                var response = await service.GetAllNewsDataByUserId(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving news for the user with Id: {UserId}", userId);
                
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    error = "An error occurred while processing your request.",
                });
            }
        }
    }
}