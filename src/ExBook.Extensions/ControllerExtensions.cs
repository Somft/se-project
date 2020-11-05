﻿using Microsoft.AspNetCore.Mvc;

namespace ExBook.Extensions
{
    public static class ControllerExtensions
    {
        public static RedirectResult RedirectToHome(this ControllerBase controller)
        {
            return controller.Redirect("/");
        }

        public static RedirectResult RedirectToLogin(this ControllerBase controller)
        {
            return controller.Redirect("/login");
        }
        public static RedirectResult RedirectToSearch(this ControllerBase controller)
        {
            return controller.Redirect("/search");
        }
    }
}
