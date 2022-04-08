using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Models.Groups
{
    public class GroupInputModel
    {
        
        public string Id { get; set; }

        public string Name { get; set; }

        [MinLength(5, ErrorMessage = "Description should be at lest 5 characters long.")]
        [MaxLength(500, ErrorMessage = "Description should be under 500 characters long.")]
        public string Description { get; set; }

        public bool IsPrivate { get; set; }
    }
}
