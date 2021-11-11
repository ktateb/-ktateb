using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common.Services;
using DAL.Entities.Countries;
using DAL.Repositories;
using Model.Country.Outputs;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly IGenericRepository<Country> _countryRepository;
        private readonly IMapper _mapper;
        public CountryService(IGenericRepository<Country> countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<ResultService<List<CountryOutput>>> GetCountries()
        {
            var result = new ResultService<List<CountryOutput>>();
            try
            {
                result.Result = _mapper.Map<List<Country>, List<CountryOutput>>(await _countryRepository.GetListAsync());
                result.Code = ResultStatusCode.Ok;
                result.Messege = "Success";
                return result;
            }
            catch
            {
                result.Result = null;
                result.Code = ResultStatusCode.InternalServerError;
                result.Messege = "Exception happen when get country";
                return result;
            }
        }


        public async Task<ResultService<CountryOutput>> GetCountry(int id)
        {
            var result = new ResultService<CountryOutput>();
            try
            {
                result.Result = _mapper.Map<Country, CountryOutput>(await _countryRepository.FindAsync(id));
                result.Code = ResultStatusCode.Ok;
                result.Messege = "Success";
                return result;
            }
            catch
            {
                result.Result = null;
                result.Code = ResultStatusCode.InternalServerError;
                result.Messege = "Exception happen when get country";
                return result;
            }
        }

    }
    public interface ICountryService
    {
        public Task<ResultService<CountryOutput>> GetCountry(int id);
        public Task<ResultService<List<CountryOutput>>> GetCountries();

    }
}