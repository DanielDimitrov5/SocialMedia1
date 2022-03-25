namespace SocialMedia1.Models
{
    public class GroupViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Members { get; set; }

        public int JoinRequests { get; set; }

        public string Status { get; set; } //IsPublic

        public string Creator { get; set; }

        public string CreatorId { get; set; }

        public CreatePostViewModel CreatePost { get; set; }

        public ICollection<ProfileViewModel> Profiles { get; set; }

        public ICollection<PostViewModel> Posts { get; set; }
    }
}
