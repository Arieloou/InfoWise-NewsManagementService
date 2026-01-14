using Microsoft.AspNetCore.Mvc;
using NewsManagementService.Application;
using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Interfaces.Repositories;

namespace NewsManagementService.Controllers
{
    [ApiController]
    [Route("subscribers")]
    public class SubscribersController(NewsAppService service) : ControllerBase
    {
        /// <summary>
        /// Endpoint utilizado exclusivamente por N8N para obtener el lote de noticias
        /// y los correos de los suscriptores para el envío diario.
        /// </summary>
        [HttpGet("newsletter-batch")]
        public async Task<ActionResult<FormatedCategoryDto>> GetNewsletterBatchForN8N()
        {
            var response = await service.GetNewsDataForN8N();
        
            if (response.NewsCategoryDtos.Count == 0)
            {
                return Content("No news data found for any category.");
            }

            return Ok(response);
        }

        [HttpGet("newsletter-batch/filter-by-shipping-hour/{hour}")]
        public async Task<ActionResult<FormatedCategoryDto>> GetNewsDataForN8N(int hour)
        {
            var response = await service.GetNewsDataForN8NByHour(hour);

            if (response.NewsCategoryDtos.Count == 0)
            {
                return Content("No news data found for any category on this schedule.");
            }
            
            return Ok(response);
        }
    }
}