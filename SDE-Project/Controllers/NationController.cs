using Microsoft.AspNetCore.Mvc;
using SDE_Project.Models;
using SDE_Project.SQLite;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SDE_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationController : ControllerBase
    {
        // GET: api/<NationController>
        [HttpGet]
        public List<Nation> Get()
        {
            DatabaseController database = new DatabaseController();
            return database.GetAllNation();
        }

        // GET api/<NationController>/5
        [HttpGet("{NationCode}")]
        public string Get(string NationCode)
        {
            DatabaseController database = new DatabaseController();
            return database.GetNationByCode(NationCode).NationDescription;
        }

        // GET api/<NationController>/NationDescription/5
        [HttpGet("/NationDescription/{NationCode}")]
        public Nation GetNation(string NationCode)
        {
            DatabaseController database = new DatabaseController();
            return database.GetNationByCode(NationCode);
        }

        // POST api/<NationController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<NationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<NationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
