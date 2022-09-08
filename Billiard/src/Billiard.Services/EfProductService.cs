﻿using Billiard.DataLayer.Context;
using Billiard.Entities;
using Billiard.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Billiard.Services;

public class EfProductService : IProductService
{
    private readonly DbSet<Product> _products;
    private readonly IUnitOfWork _uow;

    public EfProductService(IUnitOfWork uow)
    {
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        _products = _uow.Set<Product>();
    }

    public void AddNewProduct(Product product)
    {
        _uow.Set<Product>().Add(product);
    }

    public IList<Product> GetAllProducts()
    {
        return _products.Include(x => x.Category).ToList();
    }
}