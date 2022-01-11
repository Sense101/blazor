using Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;

namespace Blazor.Pages
{
    public partial class BasketPage
    {
        [Inject] AppDbContext db { get; set; }
        [Inject] NavigationManager navManager { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "name-filter")]
        public string _nameFilter { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "desc-filter")]
        public string _descFilter { get; set; }

        // I would prefer to have an input within each product but that would require binding new variables to each product in the list -- This could be done
        private int _newItemAmount = 1;

        private Basket _basket;
        private List<Product> _products = new();
        private List<BasketItem> _basketItems = new();

        protected override async Task OnInitializedAsync()
        {
            if (_nameFilter == null) { _nameFilter = ""; }
            if (_descFilter == null) { _descFilter = ""; }

            _products = await db.Products.Where(p => p.Name.ToLower().Contains(_nameFilter.ToLower()) && p.Description.ToLower().Contains(_descFilter.ToLower())).ToListAsync();


            // since there is only one basket in the database for now, I don't have a way of choosing which one is ours
            _basket = await db.Baskets.Include(b => b.BasketItems).FirstOrDefaultAsync();
            _basketItems = await db.BasketItems.Include(b => b.Product).ToListAsync();
        }

        private async void AddToBasket(Product product)
        {
            if (_newItemAmount < 1) { return; }

            // make sure there is a basket
            if (_basket == null)
            {
                _basket = new();
                db.Baskets.Add(_basket);
            }

            BasketItem existingItem = _basketItems.FirstOrDefault(item => item.Product == product);
            if (existingItem == null)
            {
                // create a new basket item entity
                BasketItem newItem = new BasketItem { Product = product, ProductId = product.Id, Quantity = _newItemAmount };

                Console.WriteLine($"Adding product to basket, id: {product.Id}");

                // add the new basket item to local and the database
                _basketItems.Add(newItem);
                db.BasketItems.Add(newItem);
            }
            else
            {
                existingItem.Quantity += _newItemAmount;
            }


            await UpdateTotalAndSave();
        }

        private async void RemoveFromBasket(BasketItem item)
        {
            // remove the item
            _basketItems.Remove(item);

            db.BasketItems.Remove(item);

            await UpdateTotalAndSave();
        }

        private async void RemoveAmount(BasketItem item, int amount)
        {
            int remainder = item.Quantity - amount;
            if (remainder < 1)
            {
                RemoveFromBasket(item);
                return;
            }
            item.Quantity = remainder;
            await UpdateTotalAndSave();
        }

        private async Task UpdateTotalAndSave()
        {
            float total = 0;
            foreach (BasketItem item in _basketItems)
            {
                total += item.Product.Price * item.Quantity;
            }

            _basket.TotalPrice = $"£{total.ToString("0.00")}";

            await db.SaveChangesAsync();
        }

        private async void SearchDatabase()
        {
            Dictionary<string, string> query = new();

            if (_nameFilter != "")
            {
                query.Add("name-filter", _nameFilter);
            }
            if (_descFilter != "")
            {
                query.Add("desc-filter", _descFilter);
            }

            // for some reason navManager.Uri isn't an full uri, so make one
            Uri uri = new(navManager.Uri);

            navManager.NavigateTo(QueryHelpers.AddQueryString(uri.AbsolutePath, query));

            _products = await (
            from product in db.Products
            where product.Name.ToLower().Contains(_nameFilter.ToLower())
            where product.Description.ToLower().Contains(_descFilter.ToLower())
            select product
            ).ToListAsync();
        }
    }
}