using Microsoft.AspNetCore.Mvc;
using StockWebApp.Model;

namespace StockWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {
        private IReadOnlyList<Company> companies;

        public CompaniesController(Data data)
        {
            companies = data.Stocks.Select(s => s.Company).ToList();
        }

        [HttpGet(Name = "GetCompanies")]
        public IEnumerable<Company> Get() => companies;
    }
}