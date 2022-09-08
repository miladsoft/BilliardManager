using Billiard.Entities;

namespace Billiard.Services.Contracts;

public interface IProductService
{
    void AddNewProduct(Product product);
    IList<Product> GetAllProducts();
}