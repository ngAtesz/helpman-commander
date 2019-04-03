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
    [Route("api/competitions/{competitionId}/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly ICompetitionRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public StationsController(ICompetitionRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<StationModel[]>> Get(int competitionId)
        {
            try
            {
                var stations = await _repository.GetAllStationByCompetitionAsync(competitionId);
                return Ok(_mapper.Map<StationModel[]>(stations));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<StationModel>> Get(int competitionId, int id)
        {
            try
            {
                var result = await _repository.GetStationByIdAsync(competitionId, id);
                if (result == null) return NotFound("Station not found.");

                return Ok(_mapper.Map<StationModel>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<StationModel>> Post(int competitionId, StationModel model)
        {
            try
            {
                var competition = await _repository.GetCompetitionAsync(competitionId);
                if (competition == null)
                {
                    return BadRequest("Competition does not exist.");
                }

                var station = _mapper.Map<Station>(model);
                station.Competition = competition;
                _repository.Add(station);

                if (await _repository.SaveChangesAsync())
                {
                    var url = _linkGenerator.GetPathByAction(HttpContext,
                                                            "Get",
                                                            values: new { competitionId, id = station.Id });
                    return Created(url, _mapper.Map<StationModel>(station));
                }
                else
                {
                    return BadRequest("Failed to save station.");
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Station couldn't be saved.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int competitionId, StationModel model, int id)
        {
            try
            {
                var station = await _repository.GetStationByIdAsync(competitionId, id);
                if (station == null) return NotFound("Station not found.");

                _mapper.Map(model, station, opt => opt.AfterMap((from, to) => { to.Id = id; }));

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<StationModel>(station));
                }
                return BadRequest("Station couldn't be updated.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int competitionId, int id)
        {
            try
            {
                var station = await _repository.GetStationByIdAsync(competitionId, id);
                if (station == null) return NotFound("Station not found.");

                _repository.Delete(station);

                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Station couldn't be deleted");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }
    }
}
