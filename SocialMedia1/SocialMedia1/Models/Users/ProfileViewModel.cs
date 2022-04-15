using SocialMedia1.Models.Posts;
using SocialMedia1.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Models.Users
{
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
            Posts = new HashSet<PostViewModel>();
        }

        public string Id { get; set; }

        public string Nickname { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        [Display(Name = "Private Profile")]
        public bool IsPrivate { get; set; }

        public string? Bio { get; set; }

        public ICollection<PostViewModel> Posts { get; set; }

        public ICollection<PostViewModel> GroupPosts { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        [AllowedExtensions(new string[] { ".jpg", ".png", ".gif" })]
        public IFormFile? Image { get; set; }

        public string ImageUrl { get; set; }
    }
}
