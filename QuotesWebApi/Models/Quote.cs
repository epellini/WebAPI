using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuotesWebApi.Models
{
    public class Quote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Author { get; set; }
        [Required]
        public string? Text { get; set; }
        public List<Like> Likes { get; set; } = new List<Like>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
