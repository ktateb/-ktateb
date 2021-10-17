using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities.Countries;
using DAL.Repositories;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly IGenericRepository<Country> _countryRepository;
        public CountryService(IGenericRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<List<Country>> GetCountries() =>
            await _countryRepository.GetListAsync();

        public async Task<Country> GetCountry(int id) =>
            await _countryRepository.FindAsync(id);
    }
    public interface ICountryService
    {
        public Task<Country> GetCountry(int id);
        public Task<List<Country>> GetCountries();

    }
}