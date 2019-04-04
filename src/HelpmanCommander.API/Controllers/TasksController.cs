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
        [HttpGet()]
        public async Task<ActionResult<TaskModel[]>> Get()
        {
            try
            {
                var tasks = await _repository.GetAllTasksAsync();
                return Ok(_mapper.Map<TaskModel[]>(tasks));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure.");
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskModel>> Get(int id)
        {
            try
            {
                var task = await _repository.GetTaskByIdAsync(id);
                if (task == null) return NotFound("Task not found.");

                return Ok(_mapper.Map<TaskModel>(task));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure.");
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<TaskModel>> Post(TaskModel model)
        {
            try
            {
                var task = _mapper.Map<Task>(model);
                _repository.Add(task);

                if (await _repository.SaveChangesAsync())
                {
                    var location = _linkGenerator.GetPathByAction("Get", "Tasks", new { id = task.Id });
                    return Created(location, _mapper.Map<TaskModel>(task));
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Task couldn't be saved.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(TaskModel model, int id)
        {
            try
            {
                var task = await _repository.GetTaskByIdAsync(id);
                if (task == null) return NotFound("Task not found.");

                _mapper.Map(model, task, opt => opt.AfterMap((from, to) =>
                                                            {
                                                                to.Id = id;
                                                                to.PrerequisiteTask = null;
                                                            }));

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<TaskModel>(task));
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Task couldn't be updated.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure.");
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var task = await _repository.GetTaskByIdAsync(id);
                if (task == null) return NotFound("Task not found");

                _repository.Delete(task);

                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Task couldn't be deleted.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure.");
            }
        }
    }
}
