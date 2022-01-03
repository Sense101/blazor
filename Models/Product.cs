using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blazor.Models;

public class Product
{
    public int Id { get; set; }
    public bool Active { get; set; } = true;
    [Required]
    public string Name { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public string Description { get; set; }
    public List<BasketItem> BasketItems { get; set; }
}
