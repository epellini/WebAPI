using Microsoft.AspNetCore.Mvc;
using QuotesWebApi.Models;
using QuotesWebApi.Services;

namespace QuotesWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : Controller
    {
        private readonly IQuotesService _quotesService;
        private readonly ILogger<TagsController> _logger;

        public TagsController(IQuotesService quotesService, ILogger<TagsController> logger)
        {
            _quotesService = quotesService;
            _logger = logger;
        }

        [HttpGet("{quoteId}")]
        public IActionResult GetTags(int quoteId)
        {
            try
            {
                var tags = _quotesService.GetTagsByQuoteId(quoteId);
                if (tags == null)
                {
                    _logger.LogWarning("No tags found for quote with ID: {QuoteId}", quoteId);
                    return NotFound($"No tags found for quote with ID: {quoteId}.");
                }
                return Ok(tags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving tags for quote with ID: {QuoteId}", quoteId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
            }
        }

        [HttpPost("{quoteId}")]
        public IActionResult AddTagToQuote(int quoteId, [FromBody] TagDto tagDto)
        {
            try
            {
                Tag tag;

                if (tagDto.Id > 0)
                {
                    // Existing tag - find it by ID
                    tag = _quotesService.FindTagById(tagDto.Id);
                    if (tag == null)
                    {
                        return NotFound("Tag not found.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(tagDto.Name))
                {
                    // New tag - create it
                    tag = new Tag { Name = tagDto.Name };
                    _quotesService.CreateTag(tag.Name);
                }
                else
                {
                    return BadRequest("Invalid tag data.");
                }

                _quotesService.AddTagToQuote(quoteId, tag.Name);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Quote not found with ID: {QuoteId}", quoteId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a tag to the quote with ID: {QuoteId}", quoteId);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{quoteId}")]
        public IActionResult RemoveTagFromQuote(int quoteId, [FromBody] Tag tag)
        {
            try
            {
                _quotesService.RemoveTagFromQuote(quoteId, tag);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Quote or Tag not found with ID: {QuoteId}", quoteId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing a tag from the quote with ID: {QuoteId}", quoteId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public IActionResult GetAllTags()
        {
            try
            {
                var tags = _quotesService.GetAllTags();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all tags.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
            }
        }
    }
}
