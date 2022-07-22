using Microsoft.AspNetCore.Mvc;
using StockWebApp.Dtos;

namespace StockWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {

        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(ILogger<CompaniesController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetCompanies")]
        public IEnumerable<Company> Get()
        {
            return new[]
            {
                new Company("AAPL", "NASDAQ", "Apple Inc."),
                new Company("GOOG", "NASDAQ", "Alphabet Inc Class C"),
                new Company("NFLX", "NASDAQ", "Netflix"),
            };
        }
    }
}