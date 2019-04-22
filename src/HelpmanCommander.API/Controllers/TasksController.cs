using System;
using System.Threading.Tasks;
using AutoMapper;
using HelpmanCommander.API.Models;
using HelpmanCommander.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Task = HelpmanCommander.Data.Entities.Task;

namespace HelpmanCommander.API.Controllers
{
    /// <summary>
    /// Managing tasks which can be assigned to exercises.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly ICompetitionRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        /// <summary>
        /// Initilizes a new instance of the <see cref="TasksController" /> class
        /// with ICompetitionRepository, AutoMapper and LinkGenerator.
        /// </summary>
        /// <param name="repository">ICompetitionRepository</param>
        /// <param name="mapper">AutoMapper</param>
        /// <param name="linkGenerator">LinkGenerator</param>
        public TasksController(ICompetitionRepository repository,
                                IMapper mapper,
                                LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// List all task.
        /// </summary>
        /// <returns>Returns all task in an array. </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
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

        /// <summary>
        /// Find a task by id.
        /// </summary>
        /// <param name="id">The id of task to return.</param>
        /// <returns>Returns a single <see cref="TaskModel"/></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Create a new task.
        /// </summary>
        /// <param name="model"><see cref="TaskModel"/> object that needs to be saved.</param>
        /// <returns>The created <see cref="TaskModel"/>.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Update an existing task.
        /// </summary>
        /// <param name="model">Updated <see cref="TaskModel"/>.</param>
        /// <param name="id">ID of the task that needs to be updated.</param>
        /// <returns>Updated <see cref="TaskModel"/>.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Deletes a task.
        /// </summary>
        /// <param name="id">The id of the task that needs to be deleted.</param>
        /// <returns>No content in case of success.</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
