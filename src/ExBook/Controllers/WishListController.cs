using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models.Authentication;
using ExBook.Models.WhishList;
﻿using ExBook.Extensions;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Controllers
{
    [Authorize]
    public class WishListController : Controller
    {
        
        private readonly WishListService wishListService;
        private ApplicationDbContext dbContext;
        public WishListController(WishListService wishListService, ApplicationDbContext dbContext)
        {
            this.wishListService = wishListService;
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("/wishlist")]
        public async Task<IActionResult> Index()
        {
           Guid userID = GetUserID();
           var result = await wishListService.GetUserBook(userID);
           return View (new WishListViewModel() {Books = result });
          
        }
       
        [HttpPost]
        [Route("/addToList")]
        public IActionResult Add()
        {
            //this.HttpContext.Response.Cookies.Delete("Authentication");
            return this.RedirectToAddToWishList();
        }

        private Guid GetUserID()
        {
            string login = this.HttpContext.User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            return dbContext.Users.Where(user => user.Login == login).Single().Id;
        }

       
        


    }
}
