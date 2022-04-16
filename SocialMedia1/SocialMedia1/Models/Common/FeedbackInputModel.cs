using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Models.Common
{
    public class FeedbackInputModel
    {
        [MinLength(10, ErrorMessage = GlobalConstants.FeedbackFormErrorMessage)]
        [MaxLength(600)]
        public string Message { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
