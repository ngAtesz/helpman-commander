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
    /// Managing competitions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionsController : ControllerBase
    {
        private readonly ICompetitionRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        /// <summary>
        /// Initilizes a new instance of the <see cref="CompetitionsController" /> class
        /// with ICompetitionRepository, AutoMapper and LinkGenerator.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        /// <param name="linkGenerator"></param>
        public CompetitionsController(ICompetitionRepository repository,
                                        IMapper mapper,
                                        LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// List all competition.
        /// </summary>
        /// <returns>Array of <see cref="CompetitionModel"/>.</returns>
        /// <response code="200">Returns all competition in an array.</response>
        [ProducesResponseType(typeof(CompetitionModel[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<CompetitionModel[]>> Get()
        {
            try
            {
                var results = await _repository.GetAllCompetitionAsync();
                return Ok(_mapper.Map<CompetitionModel[]>(results));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        /// <summary>
        /// Find competition by id.
        /// </summary>
        /// <param name="id">The id of competition to return.</param>
        /// <returns>Returns a single <see cref="CompetitionModel"/></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompetitionModel>> Get(int id)
        {
            try
            {
                var result = await _repository.GetCompetitionAsync(id, true);
                if (result == null) return NotFound("Competition not found.");

                return Ok(_mapper.Map<CompetitionModel>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        /// <summary>
        /// Add a new competition.
        /// </summary>
        /// <param name="model">CompetitionModel object needs to be added.</param>
        /// <returns>Created <see cref="CompetitionModel"/>.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<CompetitionModel>> Post(CompetitionModel model)
        {
            try
            {
                var competition = _mapper.Map<Competition>(model);
                competition.Owner = null;
                _repository.Add(competition);
                if (await _repository.SaveChangesAsync())
                {
                    var location = _linkGenerator.GetPathByAction("Get", "Competitions", new { id = competition.Id });
                    return Created(location, _mapper.Map<CompetitionModel>(competition));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Competition couldn't be saved.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        /// <summary>
        /// Update an existing competition
        /// </summary>
        /// <param name="model">Updated competition model.</param>
        /// <param name="id">Id of competition that needs to be updated.</param>
        /// <returns>Updated <see cref="CompetitionModel"/>.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(CompetitionModel model, int id)
        {
            try
            {
                var competition = await _repository.GetCompetitionAsync(id);
                if (competition == null) return NotFound("Copmetition not found.");

                _mapper.Map(model, competition, opt => opt.AfterMap((from, to) => { to.Id = id; }));

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<CompetitionModel>(competition));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Competition couldn't be updated.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        /// <summary>
        /// Deletes a competition.
        /// </summary>
        /// <param name="id">Competition id to delete.</param>
        /// <returns>No content in case of success.</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var competition = await _repository.GetCompetitionAsync(id);
                if (competition == null) return NotFound();

                _repository.Delete(competition);
                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Competition couldn't be deleted.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }
    }
}