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
            return View(new AdministrationPanelViewModel
            {
                Users = await administrationPanelService.GetUsers(),
                Transactions = await administrationPanelService.GetTransactions(),
                Ratings = await administrationPanelService.GetRatings()
            });
        }
    }
}
