namespace SocialMedia1.Models
{
    public class GroupViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Members { get; set; }

        public ICollection<PostViewModel> Posts { get; set; }
    }
}
