﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExBook.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return this.View();
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
