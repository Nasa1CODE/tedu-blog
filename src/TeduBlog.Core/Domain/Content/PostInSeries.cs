using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeduBlog.Core.Domain.Content
{
    [Table("PostInSeries")]
    [Index(nameof(PostId), nameof(SeriesId))]
    public class PostInSeries
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid SeriesId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
