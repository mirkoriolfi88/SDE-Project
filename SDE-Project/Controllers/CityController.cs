using Microsoft.AspNetCore.Mvc;
using SDE_Project.Models;
using SDE_Project.Response;
using SDE_Project.SQLite;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SDE_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        // GET: api/<CityController>
        [HttpGet]
        public List<City> Get()
        {
            DatabaseController database = new DatabaseController();
            return database.GetAllCities();
        }

        // GET api/<CityController>/5
        [HttpGet("{CodeCity}")]
        public CityWithCurrentWeather Get(string CodeCity)
        {
            DatabaseController database = new DatabaseController();
            return database.GetCity(CodeCity);
        }

        // GET api/<CityController>/CityByNation/5
        [HttpGet("/api/CityByNation/{NationCode}")]
        public List<City> GetCityByNation(string NationCode)
        {
            DatabaseController database = new DatabaseController();
            return database.GetAllCitiesByNation(NationCode);
        }

        // GET api/<CityController>/CityByNation/5
        [HttpGet("/api/NationOfCity/{CityCode}")]
        public NationOfCity GetNationOfCity(string CityCode)
        {
            DatabaseController database = new DatabaseController();
            return database.GetCityNation(CityCode);
        }

        // POST api/<CityController>
        [HttpPost]
        public CityInsertionResponse Post([FromBody] City value)
        {
            DatabaseController database = new DatabaseController();
            CityInsertionResponse response = database.InsertCity(value);

            return response;
        }

        // PUT api/<CityController>
        [HttpPut("{ID}")]
        public void Put(int ID, [FromBody] City value)
        {
            DatabaseController database = new DatabaseController();
            _ = database.UpdateCity(ID, value);
        }

        // DELETE api/<CityController>/5
        [HttpDelete("{ID}")]
        public void Delete(int ID)
        {
            DatabaseController database = new DatabaseController();
            _ = database.DeleteCity(ID);
        }
    }
}
