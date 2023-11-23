using QuotesWebApi.Models;

namespace QuotesWebApi.Services
{
    public interface IQuotesService
    {

        //Quote Methods
        IEnumerable<Quote> GetAllQuotes();
        Quote GetQuoteById(int id);
        void AddQuote(Quote quote);
        void UpdateQuote(Quote quote);
        void DeleteQuote(int id);

        // Methods for handling likes
        void AddLike(int quoteId);
        void RemoveLike(int quoteId);


        // Methods for handling tags
        IEnumerable<Tag> GetAllTags();
        IEnumerable<Tag> GetTagsByQuoteId(int quoteId);
        void RemoveTagFromQuote(int quoteId, Tag tag);

        // Additional methods for tags

        void AddTagToQuote(int quoteId, string tagName);
        Tag FindTagById(int id); // Implement this method if needed
        void CreateTag(string tagName); // Implement this method if needed
    }
}
