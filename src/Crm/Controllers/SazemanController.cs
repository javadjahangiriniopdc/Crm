
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using Crm.Domain;
using Crm.Crosscutting.Exceptions;
using Crm.Dto;
using Crm.Domain.Services.Interfaces;
using Crm.Web.Extensions;
using Crm.Web.Filters;
using Crm.Web.Rest.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Crm.Controllers {
    [Authorize]
    [Route("api")]
    [ApiController]
    public class SazemanController : ControllerBase {
        private const string EntityName = "sazeman";
        private readonly IMapper _mapper;
        private readonly ISazemanService _sazemanService;
        private readonly ILogger<SazemanController> _log;

        public SazemanController(ILogger<SazemanController> log,
            IMapper mapper,
            ISazemanService sazemanService)
        {
            _log = log;
            _mapper = mapper;
            _sazemanService = sazemanService;
        }

        [HttpPost("sazemen")]
        [ValidateModel]
        public async Task<ActionResult<SazemanDto>> CreateSazeman([FromBody] SazemanDto sazemanDto)
        {
            _log.LogDebug($"REST request to save Sazeman : {sazemanDto}");
            if (sazemanDto.Id != 0)
                throw new BadRequestAlertException("A new sazeman cannot already have an ID", EntityName, "idexists");

            Sazeman sazeman = _mapper.Map<Sazeman>(sazemanDto);
            await _sazemanService.Save(sazeman);
            return CreatedAtAction(nameof(GetSazeman), new { id = sazeman.Id }, sazeman)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, sazeman.Id.ToString()));
        }

        [HttpPut("sazemen")]
        [ValidateModel]
        public async Task<IActionResult> UpdateSazeman([FromBody] SazemanDto sazemanDto)
        {
            _log.LogDebug($"REST request to update Sazeman : {sazemanDto}");
            if (sazemanDto.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");

            //TODO catch //DbUpdateConcurrencyException into problem

            Sazeman sazeman = _mapper.Map<Sazeman>(sazemanDto);
            await _sazemanService.Save(sazeman);
            return Ok(sazeman)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, sazeman.Id.ToString()));
        }

        [HttpGet("sazemen")]
        public async Task<ActionResult<IEnumerable<SazemanDto>>> GetAllSazemen(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of Sazemen");
            var result = await _sazemanService.FindAll(pageable);
            var page = new Page<SazemanDto>(result.Content.Select(entity => _mapper.Map<SazemanDto>(entity)).ToList(),pageable,result.TotalElements);
            return Ok(((IPage<SazemanDto>)page).Content).WithHeaders(page.GeneratePaginationHttpHeaders());
        }

        [HttpGet("sazemen/{id}")]
        public async Task<IActionResult> GetSazeman([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get Sazeman : {id}");
            var result = await _sazemanService.FindOne(id);
            SazemanDto sazemanDto = _mapper.Map<SazemanDto>(result);
            return ActionResultUtil.WrapOrNotFound(sazemanDto);
        }

        [HttpDelete("sazemen/{id}")]
        public async Task<IActionResult> DeleteSazeman([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete Sazeman : {id}");
            await _sazemanService.Delete(id);
            return Ok().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
