using System;
using System.Threading.Tasks;
using HelpmanCommander.Data;
using HelpmanCommander.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace HelpmanCommander.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionsController : Controller
    {
        private readonly ICompetitionRepository _repository;

        public CompetitionsController(ICompetitionRepository repository)
        {
            _repository = repository;
        }

        // GET: api/competitions
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Competition>> Get(int id)
        {
            try
            {
                var result = await _repository.GetCompetitionAsync(id, true);
                if (result == null) return NotFound("Competition not found.");

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }
    }
}
