namespace Blazor.Models;

public class Basket
{
    public int Id { get; set; }
    public string TotalPrice { get; set; } = "£0.00";
    public List<BasketItem> BasketItems { get; set; } = new();
}