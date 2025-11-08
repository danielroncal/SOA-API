using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOA.features.auth.models;
using SOA.features.auth.repositories;
using SOA.features.location.models;
using SOA.features.seed.data;
using System.Text.RegularExpressions;

namespace SOA.features.seed
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly ApplicationDbContext _context;

        public SeedController(UserRepository userRepository, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        [HttpPost("")]
        public async Task<IActionResult> Seed()
        {
            try
            {
                await DeleteAllTables();

                await SeedUsers();
                await SeedLocation();
                await SeedServices();
                await SeedProperties();


                return Ok(new { Message = "✅ Seed ejecutado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "❌ Error al ejecutar seed.", Error = ex.Message });
            }
        }

        private async Task DeleteAllTables()
        {
            _context.ServicesToProperties.RemoveRange(_context.ServicesToProperties);
            _context.CommercialProperties.RemoveRange(_context.CommercialProperties);
            _context.ResidentialProperties.RemoveRange(_context.ResidentialProperties);
            _context.Properties.RemoveRange(_context.Properties);
            _context.Images.RemoveRange(_context.Images);
            _context.Locations.RemoveRange(_context.Locations);
            _context.Districts.RemoveRange(_context.Districts);
            _context.Provinces.RemoveRange(_context.Provinces);
            _context.Departments.RemoveRange(_context.Departments);
            _context.Services.RemoveRange(_context.Services);
            _context.Users.RemoveRange(_context.Users);

            await _context.SaveChangesAsync();
        }

        private async Task SeedUsers()
        {
            var users = new List<SOA.features.auth.models.User>
            {
                new()
                {
                    Name = "Ángel",
                    Lastname = "Santa Cruz",
                    Email = "angel@soa.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Angel123!"),
                    Phone = "987654321",
                    BirthDate = new DateTime(2000, 5, 10),
                    Confirmed = true
                },
                new()
                {
                    Name = "Aixa",
                    Lastname = "Murayari",
                    Email = "aixa@soa.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Aixa123!"),
                    Phone = "912345678",
                    BirthDate = new DateTime(1999, 9, 25),
                    Confirmed = true
                }
            };

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }

        private async Task SeedLocation()
        {
            var department = new Department
            {
                Id = Guid.NewGuid(),
                Name = "Lima",
                Slug = "lima",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            var province = new Province
            {
                Id = Guid.NewGuid(),
                Name = "Lima",
                Slug = "lima",
                DepartmentId = department.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Provinces.AddAsync(province);
            await _context.SaveChangesAsync();

            var districts = new List<District>
            {
                new() { Id = Guid.NewGuid(), Name = "Miraflores", Slug = "miraflores", ProvinceId = province.Id, DepartmentId = department.Id },
                new() { Id = Guid.NewGuid(), Name = "San Isidro", Slug = "san-isidro", ProvinceId = province.Id, DepartmentId = department.Id },
                new() { Id = Guid.NewGuid(), Name = "Surco", Slug = "surco", ProvinceId = province.Id, DepartmentId = department.Id },
                new() { Id = Guid.NewGuid(), Name = "La Molina", Slug = "la-molina", ProvinceId = province.Id, DepartmentId = department.Id },
                new() { Id = Guid.NewGuid(), Name = "Barranco", Slug = "barranco", ProvinceId = province.Id, DepartmentId = department.Id }
            };

            districts.ForEach(d =>
            {
                d.CreatedAt = DateTime.UtcNow;
                d.UpdatedAt = DateTime.UtcNow;
            });

            await _context.Districts.AddRangeAsync(districts);
            await _context.SaveChangesAsync();

            var locations = new List<Location>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    DepartmentId = department.Id,
                    Slug = department.Slug,
                    Type = "DEPARTMENT"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    DepartmentId = department.Id,
                    ProvinceId = province.Id,
                    Slug = $"{department.Slug}-{province.Slug}",
                    Type = "PROVINCE"
                }
            };

            var districtLocations = districts.Select(d => new Location
            {
                Id = Guid.NewGuid(),
                DepartmentId = department.Id,
                ProvinceId = province.Id,
                DistrictId = d.Id,
                Slug = $"{department.Slug}-{province.Slug}-{d.Slug}",
                Type = "DISTRICT"
            });

            locations.AddRange(districtLocations);
            await _context.Locations.AddRangeAsync(locations);
            await _context.SaveChangesAsync();
        }

        private async Task SeedServices()
        {
            var services = new List<SOA.features.services.models.Service>
            {
                new() { Id = Guid.NewGuid(), ServiceName = "Electricidad", Slug = "electricidad" },
                new() { Id = Guid.NewGuid(), ServiceName = "Agua Potable", Slug = "agua-potable" },
                new() { Id = Guid.NewGuid(), ServiceName = "Internet", Slug = "internet" },
                new() { Id = Guid.NewGuid(), ServiceName = "Cable TV", Slug = "cable-tv" },
                new() { Id = Guid.NewGuid(), ServiceName = "Gas Natural", Slug = "gas-natural" },
                new() { Id = Guid.NewGuid(), ServiceName = "Limpieza", Slug = "limpieza" },
                new() { Id = Guid.NewGuid(), ServiceName = "Seguridad", Slug = "seguridad" },
                new() { Id = Guid.NewGuid(), ServiceName = "Recolección de basura", Slug = "recoleccion-basura" },
                new() { Id = Guid.NewGuid(), ServiceName = "Mantenimiento general", Slug = "mantenimiento-general" },
                new() { Id = Guid.NewGuid(), ServiceName = "Jardinería", Slug = "jardineria" }
            };

            await _context.Services.AddRangeAsync(services);
            await _context.SaveChangesAsync();
        }

        private async Task<List<Guid>> GetAllServiceIds()
        {
            return await _context.Services
                .Select(s => s.Id)
                .ToListAsync();
        }

        private async Task<List<Guid>> GetAllDistrictLocationIds()
        {
            return await _context.Locations
                .Where(l => l.Type == "DISTRICT")
                .Select(l => l.Id)
                .ToListAsync();
        }

        private async Task<List<Guid>> GetAllUserIds()
        {
            return await _context.Users
                .Select(u => u.Id)
                .ToListAsync();
        }

        private static string GenerateSlug(string text)
        {
            text = text.ToLowerInvariant().Trim();
            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");
            text = Regex.Replace(text, @"\s+", "-");
            text = Regex.Replace(text, @"-+", "-");
            return text;
        }



        private async Task SeedProperties()
        {
            var userIds = await GetAllUserIds();
            var locationIds = await GetAllDistrictLocationIds();
            var serviceIds = await GetAllServiceIds();

            if (!userIds.Any() || !locationIds.Any() || !serviceIds.Any())
                throw new Exception("Faltan datos base para crear propiedades.");

            var propertiesToSeed = SeedData.Properties;

            for (int i = 0; i < propertiesToSeed.Length; i++)
            {
                dynamic data = propertiesToSeed[i];
                var userId = userIds[i % userIds.Count];
                var locationId = locationIds[i % locationIds.Count];

                var property = new SOA.features.properties.models.Property
                {
                    Id = Guid.NewGuid(),
                    Name = data.Name,
                    Slug = GenerateSlug(data.Name),
                    PropertyType = (properties.enums.PropertyType)data.PropertyType,
                    PropertyCategory = (properties.enums.PropertyCategory)data.PropertyCategory,
                    Currency = (properties.enums.Currency)data.Currency,
                    Price = data.Price,
                    Address = data.Address,
                    Description = data.Description,
                    UserId = userId,
                    Phone = data.Phone,
                    YearBuilt = data.YearBuilt,
                    Latitude = data.Latitude,
                    Longitude = data.Longitude,
                    ExtraInfo = data.ExtraInfo,
                    LocationId = locationId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Properties.AddAsync(property);
                await _context.SaveChangesAsync();

                var randomServices = serviceIds.OrderBy(_ => Guid.NewGuid()).Take(4).ToList();
                var serviceLinks = randomServices.Select(sid => new SOA.features.properties.models.ServiceToProperty
                {
                    Id = Guid.NewGuid(),
                    PropertyId = property.Id,
                    ServiceId = sid
                }).ToList();

                await _context.ServicesToProperties.AddRangeAsync(serviceLinks);
                await _context.SaveChangesAsync();
            }
        }

    }
}
