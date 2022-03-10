using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Data.Models
{
    public class Group
    {
        public Group()
        {
            Id = Guid.NewGuid().ToString();
            Users = new HashSet<UserProfileGroup>();
            Posts = new HashSet<Post>();
            JoinRequests = new HashSet<UserGroupRequest>();
        }

        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CreaterId { get; set; }

        public bool IsPrivate { get; set; }

        public int MembersCount { get; set; }

        public ICollection<UserProfileGroup> Users { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<UserGroupRequest> JoinRequests { get; set; }
    }
}
