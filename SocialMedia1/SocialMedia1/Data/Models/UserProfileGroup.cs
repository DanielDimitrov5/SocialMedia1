namespace SocialMedia1.Data.Models
{
    public class UserProfileGroup
    {
        public string UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public string GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}
