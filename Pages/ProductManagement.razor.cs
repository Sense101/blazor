using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Blazor.Models;

namespace Blazor.Pages
{
    public partial class ProductManagement
    {
        [Inject] AppDbContext db { get; set; }
        private Product _newProduct = new();

        private List<Product> _products = new();

        protected override async Task OnInitializedAsync()
        {
            _products = await db.Products.ToListAsync();
        }

        private async void AddProduct()
        {
            // validate the inputs
            if (_newProduct.Name == "" || _newProduct.Price <= 0) { return; }

            // create a new product entity
            db.Products.Add(_newProduct);
            await db.SaveChangesAsync();

            // now that the new product is tracked and has been given an id, add it the the local list
            _products.Add(_newProduct);

            // clear the form
            _newProduct = new();
        }

        private async void RemoveProduct(Product product)
        {
            if (product.BasketItems.Count > 0)
            {
                Console.WriteLine("cannot remove product from database - has basket item dependency");
                return;
            }

            _products.Remove(product);
            db.Products.Remove(product);

            await db.SaveChangesAsync();
        }
    }
}