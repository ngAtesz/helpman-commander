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
    /// Managing stations which is a sub entity of a competition. A competition is made up of multiple stations.
    /// </summary>
    [Route("api/competitions/{competitionId}/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly ICompetitionRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        /// <summary>
        /// Initilizes a new instance of the <see cref="StationsController" /> class
        /// with ICompetitionRepository, AutoMapper and LinkGenerator.
        /// </summary>
        /// <param name="repository">ICompetitionRepository</param>
        /// <param name="mapper">AutoMapper</param>
        /// <param name="linkGenerator">LinkGenerator</param>
        public StationsController(ICompetitionRepository repository,
                                    IMapper mapper,
                                    LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// List all station for a competition.
        /// </summary>
        /// <param name="competitionId">The id of competition</param>
        /// <returns>Returns all station in an array. </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Find a station in a competition by stationId.
        /// </summary>
        /// <param name="competitionId">The id of competition.</param>
        /// <param name="id">The id of station to return.</param>
        /// <returns>Returns a single StationModel</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StationModel>> Get(int competitionId, int id)
        {
            try
            {
                var competition = await _repository.GetCompetitionAsync(competitionId);
                if (competition == null) return BadRequest("Competition does not exist.");

                var result = await _repository.GetStationByIdAsync(competitionId, id);
                if (result == null) return NotFound("Station not found.");

                return Ok(_mapper.Map<StationModel>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        /// <summary>
        /// Add a new station to a competition.
        /// </summary>
        /// <param name="competitionId">The id of competition which will be extended with new station.</param>
        /// <param name="model">StationModel object that needs to be added to the competition.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<StationModel>> Post(int competitionId, StationModel model)
        {
            try
            {
                var competition = await _repository.GetCompetitionAsync(competitionId);
                if (competition == null) return BadRequest("Competition does not exist.");

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

                return BadRequest("Failed to save station.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Station couldn't be saved.");
            }
        }

        /// <summary>
        /// Update an existing station in a competition.
        /// </summary>
        /// <param name="competitionId">The id of competition where the station belongs to.</param>
        /// <param name="model">Updated station model.</param>
        /// <param name="id">ID of the station that needs to be updated.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Deletes a station in a competition.
        /// </summary>
        /// <param name="competitionId">The id of competition where the station belongs to.</param>
        /// <param name="id">The id of the station that needs to be deleted.</param>
        /// <returns>No content in case of success.</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

                return StatusCode(StatusCodes.Status500InternalServerError, "Station couldn't be deleted.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }
    }
}
