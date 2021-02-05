using Microsoft.AspNetCore.Mvc;

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
        public static RedirectResult RedirectToBookShelf(this ControllerBase controller)
        {
            return controller.Redirect("/bookshelf");
        }

        public static RedirectResult RedirectToAdminPanel(this ControllerBase controller)
        {
            return controller.Redirect("/admin");
        }

        public static RedirectResult RedirectToInitializeTransaction(this ControllerBase controller)
        {
            return controller.Redirect("/initializetransaction");
        }
        public static RedirectResult RedirectToEMailAuthorization(this ControllerBase controller)
        {
            return controller.Redirect("/login-token-sent");
        }
    }
}
