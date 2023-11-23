using Microsoft.AspNetCore.Mvc;
using QuotesWebApi.Services;

namespace QuotesWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LikesController : ControllerBase
    {

        private readonly IQuotesService _quotesService;
        private readonly ILogger<LikesController> _logger;

        public LikesController(IQuotesService quotesService, ILogger<LikesController> logger)
        {
            _quotesService = quotesService;
            _logger = logger;
        }

        [HttpPost("{quoteId}")]
        public IActionResult AddLike(int quoteId)
        {
            try
            {
                _quotesService.AddLike(quoteId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Quote not found with ID: {QuoteId}", quoteId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a like to the quote with ID: {QuoteId}", quoteId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{quoteId}")]
        public IActionResult RemoveLike(int quoteId)
        {
            try
            {
                _quotesService.RemoveLike(quoteId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Attempted to remove a like from a quote that does not exist with ID: {QuoteId}", quoteId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing a like from the quote with ID: {QuoteId}", quoteId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
