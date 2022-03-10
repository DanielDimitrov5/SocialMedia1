using SocialMedia1.Data;
using SocialMedia1.Data.Models;
using SocialMedia1.Models;

namespace SocialMedia1.Services
{
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext context;

        public GroupService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void CreateGroup(string name, string description, bool isPrivate, string creatorId)
        {
            Group group = new Group
            {
                Name = name,
                Description = description,
                CreaterId = creatorId,
                IsPrivate = isPrivate,
                MembersCount = 1,
            };

            UserProfileGroup profileGroup = new() { GroupId = group.Id, UserProfileId = creatorId };

            context.Groups.Add(group);

            context.UserProfilesGroups.Add(profileGroup);

            context.SaveChanges();
        }

        public void CreatePost(string groupId, string userId, string content)
        {
            Group group = context.Groups.Find(groupId);

            if (group is null)
            {
                return;
            }

            Post post = new Post
            {
                UserProfileId = userId,
                Content = content,
                CreatedOn = DateTime.UtcNow,
            };

            group.Posts.Add(post);

            context.SaveChanges();
        }

        public GroupViewModel GetGroup(string id)
        {
            var group = context.Groups.Find(id);

            if (group == null)
            {
                return null;
            }

            GroupViewModel groupViewModel = new GroupViewModel
            {
                Id = id,
                Name = group.Name,
                Description = group.Description,
                Members = group.MembersCount,
                Posts = GetPosts(id),
            };

            return groupViewModel;
        }

        public ICollection<GroupViewModel> GetGroups(string userId)
        {
            var groups = context.UserProfilesGroups.Where(x => x.UserProfileId == userId).ToList();

            List<GroupViewModel> model = new();

            foreach (var group in groups)
            {
                context.Entry(group).Reference(x => x.Group).Load();

                model.Add(new GroupViewModel
                {
                    Id = group.GroupId,
                    Name = group.Group.Name,
                    Description = group.Group.Description,
                    Members = group.Group.MembersCount,
                });
            }

            return model;
        }

        public ICollection<GroupViewModel> GetGroupsBySearchTerm(string searchTerm)
        {
            var groups = context.Groups
               .Where(x => x.Name.Contains(searchTerm)
                        || x.Description.Contains(searchTerm)
                        || (x.Name + x.Description).Contains(searchTerm))
               .Select(x => new GroupViewModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   Description = x.Description,
                   Members = x.MembersCount,
                   Status = x.IsPrivate ? "Private" : "Public",
               })
               .ToList();

            return groups;
        }

        public ICollection<JoinGroupRequestViewModel> GetJoinGroupRequests(string groupId)
        {
            var group = context.Groups.Find(groupId);

            context.Entry(group).Collection(x => x.JoinRequests).Load();

            List<JoinGroupRequestViewModel> requests = new();

            foreach (var req in group.JoinRequests)
            {
                context.Entry(req).Reference(x => x.UserProfile).Load();

                var request = new JoinGroupRequestViewModel
                {
                    UserId = req.UserProfile.Id,
                    GroupId = groupId,
                    Nickname = req.UserProfile.Nickname,
                    Email = req.UserProfile.EmailAddress,
                    Bio = req.UserProfile.Bio,
                };

                requests.Add(request);
            }

            return requests;
        }

        public bool IsGroupPrivate(string groupId)
        {
            return context.Groups.FirstOrDefault(x => x.Id == groupId).IsPrivate;
        }

        public bool IsJoinRequstSent(string groupId, string userId)
        {
            return context.JoinGroupRequest.Any(x => x.UserProfileId == userId && x.GroupId == groupId);
        }

        public bool IsUserGroupMember(string userId, string groupId)
        {
            return context.UserProfilesGroups.Any(x => x.GroupId == groupId && x.UserProfileId == userId);
        }

        public void JoinGroup(string groupId, string userId)
        {
            Group group = context.Groups.Find(groupId);

            UserProfile user = context.UserProfiles.Find(userId);

            if (group == null || user == null)
            {
                return;
            }

            context.UserProfilesGroups.Add(new UserProfileGroup { GroupId = groupId, UserProfileId = userId });

            group.MembersCount++;

            context.SaveChanges();
        }

        public void LeaveGroup(string groupId, string userId)
        {
            Group group = context.Groups.Find(groupId);

            UserProfileGroup user = context.UserProfilesGroups.FirstOrDefault(x => x.UserProfileId == userId && x.GroupId == groupId);

            if (group == null || user == null)
            {
                return;
            }

            group.MembersCount--;

            group.Users.Remove(user);

            context.SaveChanges();
        }

        public void SendJoinRequest(string groupId, string userId)
        {
            UserGroupRequest userGroupRequest = new UserGroupRequest
            {
                UserProfileId = userId,
                GroupId = groupId,
            };

            context.JoinGroupRequest.Add(userGroupRequest);

            context.SaveChanges();
        }

        //Private methods:

        private ICollection<PostViewModel> GetPosts(string groupId)
        {
            var posts = context.Posts.Where(x => x.GroupId == groupId).Select(x => new PostViewModel
            {
                Id = x.Id,
                AuthorId = x.UserProfileId,
                Author = x.UserProfile.Nickname,
                Content = x.Content,
                CreatedOn = x.CreatedOn,
            }).ToList();

            return posts;
        }
    }
}
