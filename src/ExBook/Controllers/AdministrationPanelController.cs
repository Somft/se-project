using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models.AdministrationPanel;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Controllers
{
    public class AdministrationPanelController : Controller
    {

        private readonly AdministrationPanelService administrationPanelService;

        public AdministrationPanelController(AdministrationPanelService administrationPanelService)
        {
            this.administrationPanelService = administrationPanelService;
        }

        [HttpGet]
        [Route("/admin")]
        [AllowAnonymous]
        public async Task<IActionResult> IndexAsync()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                    ? View("Index",new AdministrationPanelViewModel
            {
                Users = await administrationPanelService.GetUsers(),
                Transactions = await administrationPanelService.GetTransactions(),
                Ratings = await administrationPanelService.GetRatings()
            }) : this.RedirectToHome() as IActionResult;
        }

        public async Task<ActionResult> PartialViewTestAsync()
        {
            return PartialView("_EditTransaction", await administrationPanelService.GetTransactions());
        }

        public async Task<ActionResult> ShowUserDetailsAsync(string userId)
        {
            Guid id = Guid.Parse(userId);
            var user = await administrationPanelService.GetUserById(id);
            return PartialView("_UserDetailsModal", user);
        }

        public async Task<ActionResult> ShowUserTransactionsAsync(string userId)
        {
            Guid id = Guid.Parse(userId);
            var user = await administrationPanelService.GetUserById(id);
            var allTransactions = user.InitiatedTransactions.Concat(user.ReceivedTransactions).ToList();
            return PartialView("_TransactionsTab", allTransactions);
        }

        public async Task<ActionResult> DeleteUserConfirmation(string userId)
        {
            Guid id = Guid.Parse(userId);
            var user = await administrationPanelService.GetUserById(id);
            return PartialView("_DeleteUserModal", user);
        }

        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
           await administrationPanelService.DeleteUserById(userId);
            return this.RedirectToAdminPanel();
        }

        public async Task<ActionResult> DeleteTransactionConfirmation(string transactionId)
        {
            Guid id = Guid.Parse(transactionId);
            var transaction = await administrationPanelService.GetTransactionById(id);
            return PartialView("_DeleteTransactionModal", transaction);
        }

        public async Task<IActionResult> DeleteTransactionAsync(Guid transactionId)
        {
            await administrationPanelService.DeleteTransactionById(transactionId);
            return this.RedirectToAdminPanel();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUserAsync(User user)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                    await administrationPanelService.UpdateUser(user);
            }
            return await this.ShowUserDetailsAsync(user.Id.ToString());
        }
    }
}
