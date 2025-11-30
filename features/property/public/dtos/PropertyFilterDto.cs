namespace SOA.features.property.pblic.dtos
{
    public class PropertyFilterDto
    {
        public string? City { get; set; }
        public string? Type { get; set; }
        public double? Price { get; set; }
        public int? Currency { get; set; }

        // Parámetros de paginación obligatorios
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}
