using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Models
{
    public class FollowRequestViewModel
    {
        [Key]
        public string RequesterId { get; set; }

        public string ImageUrl { get; set; }

        public string Nickname { get; set; }

        public string Name { get; set; }

        public string Bio { get; set; }

        public string CurrentUserId { get; set; }
    }
}
