using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Models.Groups
{
    public class GroupInputModel
    {
        
        public string Id { get; set; }

        public string Name { get; set; }

        [MinLength(5, ErrorMessage = GlobalConstants.GroupDescpriptionMinCharException)]
        [MaxLength(500, ErrorMessage = GlobalConstants.GroupDescpriptionMaxCharException)]
        public string Description { get; set; }

        [Display(Name = "Private")]
        public bool IsPrivate { get; set; }
    }
}
