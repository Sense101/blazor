using Blazor.Models;
using Blazor.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;

namespace Blazor.Pages
{
    public partial class BasketPage
    {
        [Inject] AppDbContext db { get; set; }
        [Inject] NavigationManager navManager { get; set; }

        private SearchInput _searchInput = new();

        // I would prefer to have an input within each product but that would require binding new variables to each product in the list -- This could be done
        private int _newItemAmount = 1;

        private Basket _basket;
        private List<Product> _products = new();
        private List<BasketItem> _basketItems = new();

        protected override async Task OnInitializedAsync()
        {
            // get a proper uri
            Uri uri = new(navManager.Uri);
            Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queries = QueryHelpers.ParseQuery(uri.Query);

            if (queries.TryGetValue("name-filter", out var nameFilter))
            {
                _searchInput.NameFilter = nameFilter.ToString();
            }
            if (queries.TryGetValue("desc-filter", out var descFilter))
            {
                _searchInput.DescFilter = descFilter.ToString();
            }

            // this works fine EXCEPT when there is a query already in the url. For some reason the list of products is then never set properly
            _products = await (
            from product in db.Products
            where product.Name.ToLower().Contains(_searchInput.NameFilter.ToLower())
            where product.Description.ToLower().Contains(_searchInput.DescFilter.ToLower())
            select product
            ).ToListAsync();


            // since there is only one basket in the database for now, I don't have a way of choosing which one is ours
            _basket = await db.Baskets.Include(b => b.BasketItems).FirstOrDefaultAsync();
            if (_basket != null)
            {
                _basketItems = _basket.BasketItems;
            }
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

            if (_searchInput.NameFilter != "")
            {
                query.Add("name-filter", _searchInput.NameFilter);
            }
            if (_searchInput.DescFilter != "")
            {
                query.Add("desc-filter", _searchInput.DescFilter);
            }

            // for some reason navManager.Uri isn't an full uri, so make one
            Uri uri = new(navManager.Uri);

            navManager.NavigateTo(QueryHelpers.AddQueryString(uri.AbsolutePath, query));

            _products = await (
            from product in db.Products
            where product.Name.ToLower().Contains(_searchInput.NameFilter.ToLower())
            where product.Description.ToLower().Contains(_searchInput.DescFilter.ToLower())
            select product
            ).ToListAsync();
        }
    }
}