namespace Blazor.Models;

public class Basket
{
    public int Id { get; set; }
    public string TotalPrice { get; set; }
    public List<BasketItem> BasketItems { get; set; } = new();
}