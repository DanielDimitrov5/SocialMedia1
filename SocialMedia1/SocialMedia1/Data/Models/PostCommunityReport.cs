using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Data.Models
{
    public class PostCommunityReport
    {
        public PostCommunityReport()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        public string PostId { get; set; }

        public Post Post { get; set; }

        public string ReporterId { get; set; }

        public virtual UserProfile Reporter { get; set; }
    }
}
