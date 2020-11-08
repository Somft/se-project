using ExBook.Data;

namespace ExBook.Services
{
    public class WishListService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public WishListService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
    }
}
