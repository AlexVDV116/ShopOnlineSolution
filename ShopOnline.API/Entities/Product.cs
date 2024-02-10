using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.API.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageURL { get; set; }
    public int Price { get; set; }
    public int Qty { get; set; }
    public int CategoryId { get; set; }

    // Use ForeignKey attribute to specify the foreign key property and tell EF Core to use the CategoryId property
    // as the foreign key to join the Product entity to the ProductCategory entity 
    [ForeignKey("CategoryId")] public ProductCategory ProductCategory { get; set; }
}