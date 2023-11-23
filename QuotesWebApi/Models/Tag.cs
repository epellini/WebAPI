namespace QuotesWebApi.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public List<Quote> Quotes { get; set; } = new List<Quote>();
    }
}
