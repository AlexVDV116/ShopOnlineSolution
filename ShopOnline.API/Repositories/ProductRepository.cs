using Microsoft.EntityFrameworkCore;
using ShopOnline.API.Data;
using ShopOnline.API.Entities;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Repositories;

public class ProductRepository : IProductRepository

{
    private readonly ShopOnlineDbContext _shopOnlineDbContext;

    public ProductRepository(ShopOnlineDbContext shopOnlineDbContext)
    {
        _shopOnlineDbContext = shopOnlineDbContext;
    }

    public async Task<IEnumerable<Product>> GetItems()
    {
        var products = await _shopOnlineDbContext.Products.ToListAsync();

        return products;
    }

    public async Task<IEnumerable<ProductCategory>> GetCategories()
    {
        var categories = await _shopOnlineDbContext.ProductCategories.ToListAsync();

        return categories;
    }

    public async Task<Product> GetItem(int id)
    {
        var product = await _shopOnlineDbContext.Products.FindAsync(id);
        return product;
    }

    public async Task<ProductCategory> GetCategory(int id)
    {
        var category = await _shopOnlineDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);

        return category;
    }

    public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
    {
        var products = await (from product in _shopOnlineDbContext.Products
            where product.CategoryId == id
            select product).ToListAsync();
        return products;
    }
}