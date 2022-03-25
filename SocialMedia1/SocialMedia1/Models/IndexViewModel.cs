namespace SocialMedia1.Models
{
    public class IndexViewModel
    {
        public ICollection<PostViewModel> Posts { get; set; }

        public CreatePostViewModel CreatePost { get; set; }
    }
}
