using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Models
{
    public class JoinGroupRequestViewModel
    {
        [Key]
        public string UserId { get; set; }

        public string ImageUrl { get; set; }

        public string GroupId { get; set; }

        public string Nickname { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }
    }
}
