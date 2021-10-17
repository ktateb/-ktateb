using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using DAL.Entities.Countries;
using Microsoft.AspNetCore.Mvc;
using Model.Country.Outputs;
using Services;

namespace API.Controllers
{
    public class CountryController : BaseController
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;
        public CountryController(ICountryService countryService, IMapper mapper)
        {
            _mapper = mapper;
            _countryService = countryService;
        }

        [HttpGet("GetCountries")]
        public async Task<ActionResult<List<CountryOutput>>> GetCountries() =>
            Ok(_mapper.Map<List<Country>, List<CountryOutput>>(await _countryService.GetCountries()));

        [HttpGet("GetCountry/{id}")]
        public async Task<ActionResult<CountryOutput>> GetCountry(int id)
        {
            var dbRecord = await _countryService.GetCountry(id);
            if (dbRecord == null)
                return NotFound("This countriy is not exsit");
            return Ok(_mapper.Map<Country, CountryOutput>(dbRecord));
        }

    }
}