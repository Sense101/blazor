namespace Blazor.Models;

public class Basket
{
    public int Id { get; set; }
    public List<BasketItem> BasketItems { get; set; } = new();
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}
