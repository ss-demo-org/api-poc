using EF.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SampleApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private readonly DepartmentContext _db;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DepartmentContext departmentContext)
        {
            _logger = logger;
            _db = departmentContext;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {

            var nextPage = _db.Employees
    .OrderBy(b => b.EmployeeId)
    .Skip(50)
    .Take(10)
    .ToList();

            var result = _db.Employees.ToList();
            var query2 = _db.Employees.OrderBy(p => p.EmployeeId).Take(10); //.ToList();
            var result2 = query2.ToQueryString();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
