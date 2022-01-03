﻿using System.ComponentModel.DataAnnotations;

namespace Blazor.Models;

public class BasketItem
{
    public int Id { get; set; }
    [Required]
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    [Required]
    public Product Product { get; set; }
    [Required]
    public int BasketID { get; set; }
    [Required]
    public Basket Basket { get; set; }
}
