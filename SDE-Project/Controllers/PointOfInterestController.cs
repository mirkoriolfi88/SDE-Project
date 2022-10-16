using Microsoft.AspNetCore.Mvc;
using SDE_Project.Models;

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
            //cooming soon....
            return new List<PointOfInterest>();
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("{id}")]
        public PointOfInterest Get(int id)
        {
            //cooming soon...
            return new PointOfInterest();
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByCity/{CodeCity}")]
        public List<PointOfInterest> GetPointByCityCode(string CodeCity)
        {
            //cooming soon...
            return new List<PointOfInterest>();
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByCityAndPoint/{CodeCity}/Description/{PointDescription}")]
        public List<PointOfInterest> GetPointByCityCodeAndPointDescription(string CodeCity, string PointDescription)
        {
            //cooming soon...
            return new List<PointOfInterest>();
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByNation/{NationCode}")]
        public List<PointOfInterest> GetPointByNationCode(string NationCode)
        {
            //cooming soon...
            return new List<PointOfInterest>();
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByNationAndPoint/{NationCode}/Description/{PointDescription}")]
        public List<PointOfInterest> GetPointByNationCodeAndPointDescription(string NationCode, string PointDescription)
        {
            //cooming soon...
            return new List<PointOfInterest>();
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByLatitude/{LatitudeValue}")]
        public List<PointOfInterest> GetPointByLatitude(string LatitudeValue)
        {
            //cooming soon...
            return new List<PointOfInterest>();
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByLongitude/{LongitudeValue}")]
        public List<PointOfInterest> GetPointByLongitude(string LongitudeValue)
        {
            //cooming soon...
            return new List<PointOfInterest>();
        }

        // GET api/<PointOfInterestController>/5
        [HttpGet("/api/PointByNationAndCity/{NationCode}/City/{CityCode}")]
        public List<PointOfInterest> GetPointByNationAndCity(string NationCode, string CityCode)
        {
            //cooming soon...
            return new List<PointOfInterest>();
        }

        // POST api/<PointOfInterestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PointOfInterestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PointOfInterestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
