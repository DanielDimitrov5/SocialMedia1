﻿using SocialMedia1.Data;
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

        public GroupViewModel GetGroup(string id)
        {
            var group = context.Groups.Find(id);

            if (group == null)
            {
                return null;
            }

            var joinRequestsCount = context.Entry(group)
                           .Collection(b => b.JoinRequests)
                           .Query()
                           .Count();

            GroupViewModel groupViewModel = new GroupViewModel
            {
                Id = id,
                Name = group.Name,
                Description = group.Description,
                Members = group.MembersCount,
                JoinRequests = joinRequestsCount,
                Posts = GetPosts(id),
                Creator = context.UserProfiles.Find(group.CreaterId).Nickname,
                CreatorId = group.CreaterId
            };

            return groupViewModel;
        }

        public GroupViewModel[] GetGroups(string userId)
        {
            var groups = context.UserProfilesGroups.Where(x => x.UserProfileId == userId).ToList();

            List<GroupViewModel> model = new();

            foreach (var group in groups)
            {
                context.Entry(group).Reference(x => x.Group).Load();

                var name = group.Group.Name.Length > 40 
                    ? string.Concat(group.Group.Name.Substring(0, 40), "...") : group.Group.Name;

                var description = group.Group.Description.Length > 300 
                    ? string.Concat(group.Group.Description.Substring(0, 300), "...") : group.Group.Description;

                model.Add(new GroupViewModel
                {
                    Id = group.GroupId,
                    Name = name,
                    Description = description,
                    Members = group.Group.MembersCount,
                });
            }

            return model.ToArray();
        }

        public GroupMembersViewModel GetMembers(string groupId)
        {
            var group = context.Groups.Find(groupId);

            context.Entry(group).Collection(x => x.Users).Load();

            List<ProfileViewModel> members = new();

            foreach (var member in group.Users)
            {
                context.Entry(member).Reference(x => x.UserProfile).Load();

                var memberViewModel = new ProfileViewModel
                {
                    Id = member.UserProfileId,
                    Nickname = member.UserProfile.Nickname,
                    Bio = member.UserProfile.Bio,
                };

                members.Add(memberViewModel);
            }

            GroupMembersViewModel groupMembers = new GroupMembersViewModel
            {
                GroupCreatorId = group.CreaterId,
                GroupId = groupId,
                Members = members,
            };

            return groupMembers;
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

        // private methods:

        private ICollection<PostViewModel> GetPosts(string groupId)
        {
            var posts = context.Posts.Where(x => x.GroupId == groupId && !x.IsDeleted).Select(x => new PostViewModel
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
