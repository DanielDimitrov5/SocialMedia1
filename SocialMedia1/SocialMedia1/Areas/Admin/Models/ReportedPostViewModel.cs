using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Areas.Admin.Models
{
    public class ReportedPostViewModel
    {
        [Key]
        public string PostId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string GroupId { get; set; }

        public string GroupName { get; set; }

        public string AuthorId { get; set; }

        public string Author { get; set; }

        public int ReportsCount { get; set; }
    }
}
