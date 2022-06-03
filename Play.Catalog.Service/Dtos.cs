using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);
    public record CreateItemDto([Required, MinLength(3), MaxLength(16)] string Name, [Required, MaxLength(128)] string Description, [Required, Range(0.01, 999.999)] decimal Price);
    public record UpdateItemDto(string Name, string Description, decimal Price);

}
