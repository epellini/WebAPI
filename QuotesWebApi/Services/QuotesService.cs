using Microsoft.EntityFrameworkCore;
using QuotesWebApi.Data;
using QuotesWebApi.Models;

namespace QuotesWebApi.Services
{
    public class QuotesService : IQuotesService
    {
        private readonly ApplicationDbContext _context;

        public QuotesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Quote> GetAllQuotes()
        {
            return _context.Quotes.Include(q => q.Likes).Include(q => q.Tags).ToList();
        }

        public Quote GetQuoteById(int id)
        {
            return _context.Quotes.Include(q => q.Likes).Include(q => q.Tags)
                                  .FirstOrDefault(q => q.Id == id);
        }

        public void AddQuote(Quote quote)
        {
            // Check and process tags
            var processedTags = new List<Tag>();
            foreach (var tag in quote.Tags)
            {
                var existingTag = _context.Tags.FirstOrDefault(t => t.Name == tag.Name);
                if (existingTag != null)
                {
                    // Link existing tag
                    processedTags.Add(existingTag);
                }
                else
                {
                    // Create new tag
                    processedTags.Add(new Tag { Name = tag.Name });
                }
            }

            // Update quote with processed tags
            quote.Tags = processedTags;

            _context.Quotes.Add(quote);
            _context.SaveChanges();
        }

        public void UpdateQuote(Quote quote)
        {
            _context.Entry(quote).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteQuote(int id)
        {
            var quote = _context.Quotes.Find(id);
            if (quote == null)
            {
                throw new KeyNotFoundException($"Quote not found with ID {id}.");
            }

            _context.Quotes.Remove(quote);
            _context.SaveChanges();
        }

        public void AddLike(int quoteId)
        {
            var quote = _context.Quotes.Find(quoteId);
            if (quote == null)
            {
                throw new KeyNotFoundException($"Quote not found with ID {quoteId}.");
            }

            quote.Likes.Add(new Like());
            _context.SaveChanges();
        }

        public void RemoveLike(int quoteId)
        {
            var like = _context.Likes.FirstOrDefault(l => l.QuoteId == quoteId);
            if (like == null)
            {
                throw new KeyNotFoundException($"Like not found for quote with ID {quoteId}.");
            }

            _context.Likes.Remove(like);
            _context.SaveChanges();
        }

        public IEnumerable<Tag> GetTagsByQuoteId(int quoteId)
        {
            return _context.Quotes
                .Include(q => q.Tags)
                .FirstOrDefault(q => q.Id == quoteId)?
                .Tags;
        }

        public void AddTagToQuote(int quoteId, string tagName)
        {
            var quote = _context.Quotes.Find(quoteId);
            if (quote == null)
            {
                throw new KeyNotFoundException($"Quote not found with ID {quoteId}.");
            }

            var existingTag = _context.Tags.FirstOrDefault(t => t.Name == tagName);
            if (existingTag != null)
            {
                quote.Tags.Add(existingTag);
            }
            else
            {
                quote.Tags.Add(new Tag { Name = tagName });
            }

            _context.SaveChanges();
        }

        public void RemoveTagFromQuote(int quoteId, Tag tag)
        {
            var quote = _context.Quotes.Include(q => q.Tags).FirstOrDefault(q => q.Id == quoteId);
            if (quote == null || !quote.Tags.Any(t => t.Id == tag.Id))
            {
                throw new KeyNotFoundException($"Quote with ID {quoteId} or specified Tag not found.");
            }

            var tagToRemove = quote.Tags.FirstOrDefault(t => t.Id == tag.Id);
            quote.Tags.Remove(tagToRemove);
            _context.SaveChanges();
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return _context.Tags.ToList();
        }



        public Tag FindTagById(int id)
        {
            // Implement the logic to find a tag by its ID
            return _context.Tags.FirstOrDefault(t => t.Id == id);
        }



        public void CreateTag(string tagName)
        {
            // Implement the logic to create a new tag
            var newTag = new Tag { Name = tagName };
            _context.Tags.Add(newTag);
            _context.SaveChanges();
        }




    }
}
