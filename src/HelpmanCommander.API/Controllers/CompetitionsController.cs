using System;
using System.Threading.Tasks;
using HelpmanCommander.Data;
using HelpmanCommander.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelpmanCommander.API.Controllers
{
    [Route("api/[controller]")]
    public class CompetitionsController : Controller
    {
        private ICompetitionRepository _repository;


        public CompetitionsController(ICompetitionRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<Competition[]>> Get()
        {
            try
            {
                var results = await _repository.GetAllCompetitionAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }
    }
}
