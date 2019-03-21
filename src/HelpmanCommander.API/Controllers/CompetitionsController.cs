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
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionsController : ControllerBase
    {
        private readonly ICompetitionRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public CompetitionsController(ICompetitionRepository repository,
                                        IMapper mapper,
                                        LinkGenerator linkGenerator)
        {
            _repository = repository;
            this._mapper = mapper;
            this._linkGenerator = linkGenerator;
        }

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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompetitionModel>> Get(int id)
        {
            try
            {
                var result = await _repository.GetCompetitionAsync(id, true);
                if (result == null) return NotFound("Competition not found.");

                return _mapper.Map<CompetitionModel>(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(CompetitionModel model, int id)
        {
            try
            {
                var competition = await _repository.GetCompetitionAsync(id);
                if (competition == null) return NotFound();

                _mapper.Map(model, competition, opt => opt.AfterMap((from, to) => { to.Id = id; }));

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(competition);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Competition couldn't be updated.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CompetitionModel>> Delete(int id)
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