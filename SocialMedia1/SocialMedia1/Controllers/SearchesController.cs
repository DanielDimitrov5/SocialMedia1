using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia1.Models;
using SocialMedia1.Models.Common;
using SocialMedia1.Services.Common;

namespace SocialMedia1.Controllers
{
    public class SearchesController : Controller
    {
        private readonly ISearchService searchService;

        public SearchesController(ISearchService searchService)
        {
            this.searchService = searchService;
        }


        [Authorize]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var model = new SearchResultsViewModel
            {
                Profiles = await searchService.GetProfilesBySearchTerm(searchTerm),
                Groups = await searchService.GetGroupsBySearchTerm(searchTerm)
            };

            return View(model);
        }
    }
}
