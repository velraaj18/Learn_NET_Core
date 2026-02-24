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
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _weatherService.GetById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeather(int id, Weather weather)
        {
            var result = await _weatherService.Update(id, weather);
            if(!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await _weatherService.Delete(id);
            if(!result) return NotFound();

            return NoContent();
        }
    }
}
