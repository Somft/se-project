using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ExBook.Services
{
    public class AttachmentService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger logger;

        public AttachmentService(IConfiguration configuration, ILogger<AttachmentService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<Guid?> AddAttachment(IFormFile file)
        {       
            try
            {
                var id = Guid.NewGuid();
                string path = configuration["App:Files"];

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using Stream fileStream = new FileStream(Path.Combine(path, id.ToString()), FileMode.Create);
                await file.CopyToAsync(fileStream);
                return id;
            } 
            catch (Exception e)
            {
                logger.LogError("Unable to save file!", e);
                return null;
            }           
        }

        public async Task<FileStream?> GetAttachment(Guid id)
        {       
            try
            {
                string path = configuration["App:Files"];
                return new FileStream(Path.Combine(path, id.ToString()), FileMode.Open);
            } 
            catch
            {
                return null;
            }
        }
    }
}
