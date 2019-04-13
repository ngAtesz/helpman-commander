using System;
using System.Threading.Tasks;
using AutoMapper;
using HelpmanCommander.API.Models;
using HelpmanCommander.Data;
using HelpmanCommander.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Task = System.Threading.Tasks.Task;

namespace HelpmanCommander.API.Controllers
{
    [Route("api/competitions/{competitionId}/stations/{stationId}/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly ICompetitionRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public ExercisesController(ICompetitionRepository repository,
                                    IMapper mapper,
                                    LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

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
    }
}