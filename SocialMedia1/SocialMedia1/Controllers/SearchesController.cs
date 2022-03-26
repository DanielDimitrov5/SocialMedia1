using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia1.Models;
using SocialMedia1.Services;

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
        public IActionResult Search(string searchTerm)
        {
            var model = new SearchResultsViewModel
            {
                Profiles = searchService.GetProfilesBySearchTerm(searchTerm),
                Groups = searchService.GetGroupsBySearchTerm(searchTerm)
            };

            return View(model);
        }
    }
}
