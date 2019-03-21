using System;
using System.Threading.Tasks;
using AutoMapper;
using HelpmanCommander.API.Models;
using HelpmanCommander.Data;
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
                var stations = await _repository.GetStationsByCompetitionAsync(competitionId);
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
                var result = await _repository.GetStationAsync(id);
                if (result == null) return NotFound("Competition not found.");

                return Ok(_mapper.Map<StationModel>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }
    }
}
