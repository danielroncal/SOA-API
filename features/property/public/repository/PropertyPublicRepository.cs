using SOA.Features.Location.Repository;

namespace SOA.features.property.pblic.repository
{
    public class PropertyPublicRepository
{
        private readonly ApplicationDbContext _context;

        public PropertyPublicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
