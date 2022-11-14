using Microsoft.AspNetCore.Mvc;
using SDE_Project.Models;
using SDE_Project.Response;
using SDE_Project.SQLite;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SDE_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointOfInterestController : ControllerBase
    {
        // GET: api/<PointOfInterestController>
        [HttpGet]
        public List<PointOfInterest> Get()
        {
            DatabaseController database = new DatabaseController();
            return database.GetAllPointOfInterest();
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("{id}")]
        public PoinOfInterestWithActivity Get(int id)
        {
            DatabaseController database = new DatabaseController();
            return database.GetAllPointOfInterestById(id);
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByCity/{CodeCity}")]
        public List<PointOfInterest> GetPointByCity(int IDCity)
        {
            DatabaseController database = new DatabaseController();
            return database.GetAllPointOfInterestByCity(IDCity);
        }
        
        /*
        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByCityAndPoint/{IDCity}/Description/{PointDescription}")]
        public List<PointOfInterest> GetPointByCityAndPointDescription(int IDCity, string PointDescription)
        {
            DatabaseController database = new DatabaseController();
            return database.GetAllPointOfInterestByCityAndDescription(IDCity, PointDescription);
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByNation/{NationCode}")]
        public List<PointOfInterest> GetPointByNationCode(string NationCode)
        {
            DatabaseController database = new DatabaseController();
            return database.GetAllPointOfInterestByNation(NationCode);
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByNationAndPoint/{NationCode}/Description/{PointDescription}")]
        public List<PointOfInterest> GetPointByNationCodeAndPointDescription(string NationCode, string PointDescription)
        {
            DatabaseController database = new DatabaseController();
            return database.GetAllPointOfInterestByNationAndDescription(NationCode, PointDescription);
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByLatitude/{LatitudeValue}")]
        public List<PointOfInterest> GetPointByLatitude(string LatitudeValue)
        {
            DatabaseController database = new DatabaseController();
            return database.GetAllPointOfInterestByLatitude(LatitudeValue);
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByLongitude/{LongitudeValue}")]
        public List<PointOfInterest> GetPointByLongitude(string LongitudeValue)
        {
            DatabaseController database = new DatabaseController();
            return database.GetAllPointOfInterestByLongitude(LongitudeValue);
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByNationAndCity/{NationCode}/City/{CityCode}")]
        public List<PointOfInterest> GetPointByNationAndCity(string NationCode, string CityCode)
        {
            DatabaseController database = new DatabaseController();
            return database.GetAllPointOfInterestByNationAndCode(NationCode, CityCode);
        }

        [HttpGet("/api/AllDetailPointByNationAndCity/{NationCode}/City/{CityCode}")]
        public DetailsPointInterestResponse GetAllDetailsPointByNationAndCity(string NationCode, string CityCode)
        {
            DatabaseController database = new DatabaseController();
            return database.DetailOfPointOfInterest(NationCode, CityCode);
        }*/

        // POST api/<PointOfInterestController>
        [HttpPost]
        public void Post([FromBody] PoinInterestRequest value)
        {
            DatabaseController database = new DatabaseController();
            _ = database.InsertPointOfInterest(value);
        }

        // PUT api/<PointOfInterestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] PointOfInterest value)
        {
            DatabaseController database = new DatabaseController();
            _ = database.UpdatePointOfInterest(id, value);
        }

        // DELETE api/<PointOfInterestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            DatabaseController database = new DatabaseController();
            _ = database.DeletePointOfInterest(id);
        }
    }
}
