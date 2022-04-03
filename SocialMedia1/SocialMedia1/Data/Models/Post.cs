using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Data.Models
{
    public class Post
    {
        public Post()
        {
            Id = Guid.NewGuid().ToString();
            CommunityReports = new HashSet<PostCommunityReport>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        public string UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public string? GroupId { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<PostCommunityReport> CommunityReports { get; set; }
    }
}
