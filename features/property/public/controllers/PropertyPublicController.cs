using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOA.features.property.pblic.dtos;
using SOA.features.properties.models;
using System.Linq;
using System.Threading.Tasks;

namespace SOA.features.property.pblic.controllers
{
    [Route("api/property-public")]
    [ApiController]
    public class PropertyPublicController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PropertyPublicController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProperties([FromQuery] PropertyFilterDto filter)
        {
            if (filter.Page <= 0 || filter.Limit <= 0)
                return BadRequest("Los parámetros 'page' y 'limit' deben ser mayores a 0.");

            var query = _context.Properties
                .Include(p => p.Location)
                .Include(p => p.Images)
                .Where(p => p.Availability)
                .AsQueryable();

            if (filter.Currency.HasValue)
                query = query.Where(p => (int)p.Currency == filter.Currency.Value);

            // 📊 Paginación
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)filter.Limit);

            var skip = (filter.Page - 1) * filter.Limit;
            var properties = await query
                .Skip(skip)
                .Take(filter.Limit)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Address,
                    p.PropertyType,
                    p.PropertyCategory,
                    p.Availability,
                    p.Currency,
                    Images = p.Images.Select(i => i.Url)
                })
                .ToListAsync();

            // 📦 Construcción del resultado
            var response = new
            {
                data = properties,
                meta = new
                {
                    totalPages = totalPages,
                    itemsPerPage = filter.Limit,
                    itemsInPage = properties.Count,
                    currentPage = filter.Page,
                    nextPage = filter.Page < totalPages,
                    prevPage = filter.Page > 1
                }
            };

            return Ok(response);
        }
    }
}
