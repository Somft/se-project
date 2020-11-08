using ExBook.Data;

namespace ExBook.Services
{
    public class SearchService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public SearchService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
    }
}
