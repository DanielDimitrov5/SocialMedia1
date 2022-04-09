using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Models.Posts
{
    public class CreatePostViewModel
    {
        public string? Id { get; set; }

        public string? Content { get; set; }
    }
}
