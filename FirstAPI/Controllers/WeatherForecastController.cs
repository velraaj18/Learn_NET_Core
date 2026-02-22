using FirstAPI.Models;
using FirstAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        public WeatherForecastController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _weatherService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> PostWeather([FromBody] Weather weather)
        {
            return Ok(await _weatherService.Post(weather));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok($"Get Weather Data of Id: {id}");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            return Ok($"Delete Weather Data by Id: {id}");
        }
    }
}
