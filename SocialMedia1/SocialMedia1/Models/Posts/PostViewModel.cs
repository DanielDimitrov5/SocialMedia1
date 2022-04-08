using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Models.Posts
{
    public class PostViewModel
    {
        [Key]
        public string Id { get; set; }

        public string Author { get; set; }

        public string AuthorId { get; set; }

        public string ImageUrl { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string GroupId { get; set; }

        public string GroupName { get; set; }
    }
}
