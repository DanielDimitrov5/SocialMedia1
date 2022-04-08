using SocialMedia1.Models;
using SocialMedia1.Models.Common;

namespace SocialMedia1.Services.Common
{
    public interface IIndexService
    {
        IndexViewModel GetIndexView(string userId);
    }
}
