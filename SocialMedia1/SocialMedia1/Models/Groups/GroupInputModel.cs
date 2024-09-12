using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Models.Groups
{
    public class GroupInputModel
    {
        
        public string Id { get; set; }

        public string Name { get; set; }

        [MinLength(5, ErrorMessage = GlobalConstants.GroupDescriptionMinCharException)]
        [MaxLength(500, ErrorMessage = GlobalConstants.GroupDescriptionMaxCharException)]
        public string Description { get; set; }

        [Display(Name = "Private")]
        public bool IsPrivate { get; set; }
    }
}
