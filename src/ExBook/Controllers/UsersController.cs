using ExBook.Data;
using ExBook.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Linq;

namespace ExBook.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UsersController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            return this.View(new UsersViewModel()
            {
                Users = this.applicationDbContext.Users.ToList()
            });
        }
    }
}