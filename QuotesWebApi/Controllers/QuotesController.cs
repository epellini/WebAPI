using Microsoft.AspNetCore.Mvc;
using QuotesWebApi.Models;
using QuotesWebApi.Services;

namespace QuotesWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuotesController : ControllerBase
    {
        private readonly IQuotesService _quotesService;
        private readonly ILogger<QuotesController> _logger;

        public QuotesController(IQuotesService quotesService, ILogger<QuotesController> logger)
        {
            _quotesService = quotesService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Quote>> Get()
        {
            try
            {
                return Ok(_quotesService.GetAllQuotes());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all quotes.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Quote> Get(int id)
        {
            try
            {
                var quote = _quotesService.GetQuoteById(id);
                if (quote == null)
                {
                    _logger.LogWarning("Quote with ID {QuoteId} not found.", id);
                    return NotFound($"Quote with ID {id} not found.");
                }
                return Ok(quote);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving quote with ID {QuoteId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
            }
        }

        [HttpPost]
        public ActionResult<Quote> Post([FromBody] Quote quote)
        {
            try
            {
                _quotesService.AddQuote(quote);
                return CreatedAtAction(nameof(Get), new { id = quote.Id }, quote);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new quote.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving data to the database.");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Quote quote)
        {
            try
            {
                if (id != quote.Id)
                {
                    _logger.LogWarning("Update request ID mismatch for quote ID {QuoteId}.", id);
                    return BadRequest("ID mismatch");
                }

                _quotesService.UpdateQuote(quote);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating quote with ID {QuoteId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data in the database.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _quotesService.DeleteQuote(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Attempted to delete a quote which does not exist with ID {QuoteId}.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting quote with ID {QuoteId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data from the database.");
            }
        }

    }
}
