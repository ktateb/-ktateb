using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.DataContext;
using DAL.Entities.Countries;

namespace Services.Seed
{
    public class CountrySeed
    {
        public static async Task SeedCountryAsync(StoreContext dbContext)
        {
            if (!dbContext.Countries.Any())
            {
                var countriesData = await File.ReadAllTextAsync("../Services/Seed/Data/Countries.json");
                var countries = JsonSerializer.Deserialize<List<Country>>(countriesData);
                foreach (var country in countries)
                {
                    var countryForAdd = new Country
                    {
                        Name = country.Name,
                        Code = country.Code
                    };
                    await dbContext.AddAsync(countryForAdd);
                }
                await dbContext.SaveChangesAsync();
            }

        }

    }
}