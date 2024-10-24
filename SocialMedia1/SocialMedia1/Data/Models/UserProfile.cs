﻿using System.ComponentModel.DataAnnotations;

namespace SocialMedia1.Data.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            Posts = new HashSet<Post>();
            Follows = new HashSet<UserProfile>();
            FollowedBy  = new HashSet<UserProfile>();
            FollowRequests = new HashSet<FollowRequest>();
            Groups = new HashSet<UserProfileGroup>();
            JoinRequests = new HashSet<UserGroupRequest>();
            PostReports = new HashSet<PostCommunityReport>();

            ImageUrl = "/img/ProfilePictures/noPicturee.png";
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Nickname { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? ImageUrl { get; set; }

        public string? EmailAddress { get; set; }

        public string? Bio { get; set; }

        public bool IsPrivate { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<UserProfile> Follows { get; set; }

        public virtual ICollection<UserProfile> FollowedBy { get; set; }

        public virtual ICollection<FollowRequest> FollowRequests { get; set; }

        public virtual ICollection<UserProfileGroup> Groups { get; set; }

        public virtual ICollection<UserGroupRequest> JoinRequests { get; set; }

        public virtual ICollection<PostCommunityReport> PostReports { get; set; }
    }
}
