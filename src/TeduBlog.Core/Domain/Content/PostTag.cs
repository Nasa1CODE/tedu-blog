using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeduBlog.Core.Domain.Content
{
    [Table("PostTags")]
    [Index(nameof(PostId), nameof(TagId))]
    public class PostTag
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid TagId { get; set; }
    }
}
