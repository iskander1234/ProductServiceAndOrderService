﻿namespace ProductService.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime? DeleteDate { get; set; } // Soft Delete
}