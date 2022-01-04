using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blazor.Models;

public class Product
{
    public int Id { get; set; }
    [Required] public string Name { get; set; }
    public float Price { get; set; }
    public string Description { get; set; }
    public List<BasketItem> BasketItems { get; set; } = new();
}
