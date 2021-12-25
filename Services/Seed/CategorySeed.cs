using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.DataContext;
using DAL.Entities.Categories;

namespace Services.Seed
{
    public class CategorySeed
    {
        public static async Task SeedCategoryAsync(StoreContext dbContext)
        {
            if (!dbContext.Categories.Any())
            {
                var categoriesData = await File.ReadAllTextAsync("../Services/Seed/Data/Category.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);
                foreach (var category in categories)
                {
                    var categoryForAdd = new Category
                    {
                        CategoryName = category.CategoryName,
                    };
                    if (category.BaseCategoryID != 0)
                        categoryForAdd.BaseCategoryID = category.BaseCategoryID;

                    await dbContext.AddAsync(categoryForAdd);
                }
                await dbContext.SaveChangesAsync();
            }
        }
    }
}