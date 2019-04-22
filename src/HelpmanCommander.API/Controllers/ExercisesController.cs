using System;
using System.Threading.Tasks;
using AutoMapper;
using HelpmanCommander.API.Models;
using HelpmanCommander.Data;
using HelpmanCommander.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HelpmanCommander.API.Controllers
{
    /// <summary>
    /// Managing exercises which is a sub-entity of a station. A station is made up of multiple exercises.
    /// </summary>
    [Route("api/competitions/{competitionId}/stations/{stationId}/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly ICompetitionRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        /// <summary>
        /// Initilizes a new instance of the <see cref="ExercisesController" /> class
        /// with ICompetitionRepository, AutoMapper and LinkGenerator.
        /// </summary>
        /// <param name="repository">ICompetitionRepository</param>
        /// <param name="mapper">AutoMapper</param>
        /// <param name="linkGenerator">LinkGenerator</param>
        public ExercisesController(ICompetitionRepository repository,
                                    IMapper mapper,
                                    LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// List all exercise for a competition.
        /// </summary>
        /// <param name="competitionId">The id of competition</param>
        /// <param name="stationId">The id of station.</param>
        /// <returns>Returns all exercise in an array.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<ExerciseModel[]>> Get(int competitionId, int stationId)
        {
            try
            {
                var station = _repository.GetStationByIdAsync(competitionId, stationId);
                if (station == null) return BadRequest("Station does not exist.");

                var exercises = await _repository.GetAllExerciseByStationAsync(competitionId, stationId);
                return Ok(_mapper.Map<ExerciseModel[]>(exercises));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure.");
            }
        }

        /// <summary>
        /// Find a station in a competition by stationId.
        /// </summary>
        /// <param name="competitionId">The id of competition.</param>
        /// <param name="stationId">The id of station.</param>
        /// <param name="id">The id of station to return.</param>
        /// <returns>Returns a single StationModel</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ExerciseModel>> Get(int competitionId, int stationId, int id)
        {
            try
            {
                var station = _repository.GetStationByIdAsync(competitionId, stationId);
                if (station == null) return BadRequest("Station does not exist.");
                
                var exercise = await _repository.GetExerciseByIdAsync(id);
                if (exercise == null) return NotFound("Exercise not found.");

                return Ok(_mapper.Map<ExerciseModel>(exercise));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure.");
            }
        }

        /// <summary>
        /// Add a new exercise to a station.
        /// </summary>
        /// <param name="competitionId">The id of competition.</param>
        /// <param name="stationId">The id of station.</param>
        /// <param name="model"><see cref="ExerciseModel"/> object that needs to be added to the competition.</param>
        /// <returns>Created <see cref="ExerciseModel"/>.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<ExerciseModel>> Post(int competitionId, int stationId, ExerciseModel model)
        {
            try
            {
                var station = await _repository.GetStationByIdAsync(competitionId, stationId);
                if (station == null) return BadRequest("Station does not exist.");

                var exercise = _mapper.Map<Exercise>(model);

                exercise.Station = station;
                _repository.Add(exercise);

                if (await _repository.SaveChangesAsync())
                {
                    var url = _linkGenerator.GetPathByAction(HttpContext,
                                                            "Get",
                                                            values: new { competitionId, stationId, id = exercise.Id });
                    return Created(url, _mapper.Map<ExerciseModel>(exercise));
                }

                return BadRequest("Failed to save exercise.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure.");
            }
        }

        /// <summary>
        /// Update an existing exercise of a station.
        /// </summary>
        /// <param name="competitionId">The id of competition.</param>
        /// <param name="stationId">The id of station.</param>
        /// <param name="model">Updated <see cref="ExerciseModel"/>.</param>
        /// <param name="id">ID of the station that needs to be updated.</param>
        /// <returns>Modified <see cref="ExerciseModel"/>.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int competitionId, int stationId, ExerciseModel model, int id)
        {
            try
            {
                var station = await _repository.GetStationByIdAsync(competitionId, stationId);
                if (station == null) return BadRequest("Station does not exist.");

                var exercise = await _repository.GetExerciseByIdAsync(id);
                if (exercise == null) return NotFound("Exercise not found.");

                _mapper.Map(model, exercise, opt => opt.AfterMap((from, to) => { to.Id = id; }));

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<ExerciseModel>(exercise));
                }

                return BadRequest("Exercise couldn't be updated.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        /// <summary>
        /// Deletes an exercise of a station.
        /// </summary>
        /// <param name="competitionId">The id of competition.</param>
        /// <param name="stationId">The id of station.</param>
        /// <param name="id">The id of the exercise that needs to be deleted.</param>
        /// <returns>No content in case of success.</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int competitionId, int stationId, int id)
        {
            try
            {
                var station = await _repository.GetStationByIdAsync(competitionId, stationId);
                if (station == null) return BadRequest("Station does not exist.");

                var exercise = await _repository.GetExerciseByIdAsync(id);
                if (exercise == null) return NotFound("Exercise not found.");

                _repository.Delete(exercise);

                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Exercise couldn't be deleted.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }
    }
}