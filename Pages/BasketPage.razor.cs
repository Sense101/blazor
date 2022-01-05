using Microsoft.EntityFrameworkCore;
using Blazor.Models;

namespace Blazor.Pages
{
    public partial class BasketPage
    {
        private AppDbContext db = new();
        private Product _newProduct = new();

        // I would prefer to have an input within each product but that would require binding new variables to each prodcut in the list
        private int _newProductAmount = 1;


        private Basket _basket;
        private List<Product> _products = new();
        private List<BasketItem> _basketItems = new();
        protected override async Task OnInitializedAsync()
        {
            _products = await db.Products.ToListAsync();

            // since there is only one basket in the database for now, I don't have a way of choosing which one is ours
            _basket = await db.Baskets.Include(b => b.BasketItems).FirstOrDefaultAsync();

            // For some reason it returns an empty string for the price - even though when I pull from the db after updating it then it has the price stored.
            //I'm not sure why this is, so I'm leaving it blank at the start till I understand why.

            if (_basket == null)
            {
                _basket = new();
                db.Baskets.Add(_basket);
                UpdateTotalAndSave();
            }

            // no need to get the list of items from the database if we get it with the basket
            _basketItems = _basket.BasketItems;

        }

        private void AddProduct()
        {
            // validate the inputs
            if (_newProduct.Name == "" || _newProduct.Price <= 0) { return; }

            // create a new product entity
            db.Products.Add(_newProduct);
            db.SaveChanges();

            // now that the new product is tracked and has been given an id, add it the the local list
            _products.Add(_newProduct);

            // clear the form
            _newProduct = new();
        }

        private void RemoveProduct(Product product)
        {
            if (product.BasketItems.Count > 0)
            {
                Console.WriteLine("cannot remove product from database - has basket item dependency");
                return;
            }

            _products.Remove(product);

            db.Products.Remove(product);

            UpdateTotalAndSave();
        }

        private void AddToBasket(Product product)
        {
            if (_newProductAmount < 1) { return; }
            // create a new basket item entity
            BasketItem newItem = new BasketItem { Product = product, ProductId = product.Id, Quantity = _newProductAmount };

            Console.WriteLine($"Adding product to basket, id: {product.Id}");

            // add the new basket item to local and the database
            _basketItems.Add(newItem);
            db.BasketItems.Add(newItem);

            UpdateTotalAndSave();
        }

        private void RemoveFromBasket(BasketItem item)
        {
            // remove the item
            _basketItems.Remove(item);

            db.BasketItems.Remove(item);

            UpdateTotalAndSave();
        }

        private void RemoveAmount(BasketItem item, int amount)
        {
            int remainder = item.Quantity - amount;
            if (remainder < 1)
            {
                RemoveFromBasket(item);
                return;
            }
            item.Quantity = remainder;
            UpdateTotalAndSave();
        }

        private void UpdateTotalAndSave()
        {
            float total = 0;
            foreach (BasketItem item in _basketItems)
            {
                total += item.Product.Price * item.Quantity;
            }

            _basket.TotalPrice = $"£{total.ToString("0.00")}";

            db.SaveChanges();

            Console.WriteLine(db.Baskets.FirstOrDefault().TotalPrice);
        }
    }
}