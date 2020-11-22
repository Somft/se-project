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
        public static RedirectResult RedirectToWishList(this ControllerBase controller)
        {
            return controller.Redirect("/wishlist");
        }
        public static RedirectResult RedirectToAddToWishList(this ControllerBase controller)
        {
            return controller.Redirect("/addtowishlist");
        }
        public static RedirectResult RedirectToFinalizeTransaction(this ControllerBase controller)
        {
            return controller.Redirect("/finalizetransaction");
        }

    }
}
