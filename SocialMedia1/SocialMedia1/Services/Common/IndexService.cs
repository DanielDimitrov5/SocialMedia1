﻿using SocialMedia1.Models;
using SocialMedia1.Models.Common;
using SocialMedia1.Models.Posts;
using SocialMedia1.Services.Posts;

namespace SocialMedia1.Services.Common
{
    public class IndexService : IIndexService
    {
        private readonly IPostService postService;

        public IndexService(IPostService postService)
        {
            this.postService = postService;
        }

        public IndexViewModel GetIndexView(string userId)
        {
            var postsByFollowedUsers = postService.GetAllPostsByFollowedUsers(userId);
            var myPosts = postService.GetAllPosts(userId).Where(x => x.GroupId == null).ToList();

            var allPosts = new List<PostViewModel>();

            allPosts.AddRange(postsByFollowedUsers);
            allPosts.AddRange(myPosts);

            return new IndexViewModel
            {
                Posts = allPosts,
                GroupPosts = postService.GetAllPostsInUsersGroups(userId),
            };
        }
    }
}