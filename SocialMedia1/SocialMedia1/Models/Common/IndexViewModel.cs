using SocialMedia1.Models.Posts;

namespace SocialMedia1.Models.Common
{
    public class IndexViewModel
    {
        public ICollection<PostViewModel> Posts { get; set; }

        public ICollection<PostViewModel> GroupPosts { get; set; }

        public CreatePostViewModel CreatePost { get; set; }
    }
}
