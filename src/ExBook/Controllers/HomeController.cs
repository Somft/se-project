using ExBook.Models.Home;
using ExBook.Services;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExBook.Controllers
{
    [Controller]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly HomeService homeService;

        public HomeController(HomeService homeService)
        {
            this.homeService = homeService;
        }
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            return this.View(new HomeViewModel()
            {
                LatestBooks = await this.homeService.GetLatestBooksAsync()
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature exceptionHandler = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandler == null)
            {
                return this.StatusCode(404);
            }

            return this.View();
        }
    }
}
