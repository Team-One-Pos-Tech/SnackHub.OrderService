using System;

namespace SnackHub.OrderService.Domain.Entities;

public class Product
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

    public void Edit(string name, string description, decimal price)
    {
        Name = name;
        Description = description;
        Price = price;
    }
    
    private Product() { }
    public static Product Create(Guid id, string name, string description, decimal price)
        => new Product
        {
            Id = id,
            Name = name,
            Description = description,
            Price = price,
        };
}