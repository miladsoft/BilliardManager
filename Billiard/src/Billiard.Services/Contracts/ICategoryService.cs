using Billiard.Entities;

namespace Billiard.Services.Contracts;

public interface ICategoryService
{
    void AddNewCategory(Category category);
    IList<Category> GetAllCategories();
}