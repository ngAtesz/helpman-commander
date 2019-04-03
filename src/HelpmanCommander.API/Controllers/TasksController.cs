using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelpmanCommander.API.Models;
using HelpmanCommander.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Task = HelpmanCommander.Data.Entities.Task;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelpmanCommander.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly ICompetitionRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public TasksController(ICompetitionRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        // GET: api/<controller>
        [HttpGet("{exerciseId:int?}")]
        public async Task<ActionResult<TaskModel[]>> Get(int? exerciseId)
        {
            try
            {
                var tasks = await _repository.GetAllTasksAsync(exerciseId);
                return Ok(_mapper.Map<TaskModel[]>(tasks));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure.");
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
