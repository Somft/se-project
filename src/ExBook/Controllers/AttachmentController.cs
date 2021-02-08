using ExBook.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace ExBook.Controllers
{
    [Authorize]
    public class AttachmentController : Controller
    {
        private readonly AttachmentService attachmentService;

        public AttachmentController(AttachmentService attachmentService)
        {
            this.attachmentService = attachmentService;
        }

        [HttpGet]
        [Route("/attachment")]
        public async Task<IActionResult> GetAttachment(Guid id)
        {
            var file = await attachmentService.GetAttachment(id);

            return new FileStreamResult(file, "image/png")
            {
                FileDownloadName = id.ToString(),
            };
        }
    }
}
