using SOA.features.property.admin.dtos;
using SOA.features.properties.models;

namespace SOA.features.property.admin.repository
{
    public class PropertyAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public PropertyAdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Property> CreateAsync(CreatePropertyAdminDto dto, Guid userId, Guid locationId, String slug)
        {

            var property = new Property
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Slug = slug,
                PropertyType = dto.PropertyType,
                PropertyCategory = dto.PropertyCategory,
                Currency = dto.Currency,
                Price = (double)dto.Price,
                Address = dto.Address,
                Description = dto.Description,
                UserId = userId,
                Phone = dto.Phone,
                YearBuilt = dto.YearBuilt,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                ExtraInfo = dto.ExtraInfo,
                LocationId = locationId, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Properties.AddAsync(property);
            await _context.SaveChangesAsync();

            if (dto.ServicesId != null && dto.ServicesId.Count > 0)
            {
                var servicesToInsert = dto.ServicesId.Select(sid => new ServiceToProperty
                {
                    Id = Guid.NewGuid(),
                    PropertyId = property.Id,
                    ServiceId = sid
                }).ToList();

                await _context.ServicesToProperties.AddRangeAsync(servicesToInsert);
                await _context.SaveChangesAsync();
            }

            return property;
        }


    }
}
