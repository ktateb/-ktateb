using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Model.Country.Outputs;
using Services;

namespace API.Controllers
{
    public class CountryController : BaseController
    {
        private readonly ICountryService _countryService;
        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("GetCountries")]
        public async Task<ActionResult<List<CountryOutput>>> GetCountries() =>
            GetResult(await _countryService.GetCountries());

        [HttpGet("GetCountry/{id}")]
        public async Task<ActionResult<CountryOutput>> GetCountry(int id) =>
            GetResult(await _countryService.GetCountry(id));
    }
}