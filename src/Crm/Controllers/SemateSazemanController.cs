
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
    public class SemateSazemanController : ControllerBase {
        private const string EntityName = "semateSazeman";
        private readonly IMapper _mapper;
        private readonly ISemateSazemanService _semateSazemanService;
        private readonly ILogger<SemateSazemanController> _log;

        public SemateSazemanController(ILogger<SemateSazemanController> log,
            IMapper mapper,
            ISemateSazemanService semateSazemanService)
        {
            _log = log;
            _mapper = mapper;
            _semateSazemanService = semateSazemanService;
        }

        [HttpPost("semate-sazemen")]
        [ValidateModel]
        public async Task<ActionResult<SemateSazemanDto>> CreateSemateSazeman([FromBody] SemateSazemanDto semateSazemanDto)
        {
            _log.LogDebug($"REST request to save SemateSazeman : {semateSazemanDto}");
            if (semateSazemanDto.Id != 0)
                throw new BadRequestAlertException("A new semateSazeman cannot already have an ID", EntityName, "idexists");

            SemateSazeman semateSazeman = _mapper.Map<SemateSazeman>(semateSazemanDto);
            await _semateSazemanService.Save(semateSazeman);
            return CreatedAtAction(nameof(GetSemateSazeman), new { id = semateSazeman.Id }, semateSazeman)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, semateSazeman.Id.ToString()));
        }

        [HttpPut("semate-sazemen")]
        [ValidateModel]
        public async Task<IActionResult> UpdateSemateSazeman([FromBody] SemateSazemanDto semateSazemanDto)
        {
            _log.LogDebug($"REST request to update SemateSazeman : {semateSazemanDto}");
            if (semateSazemanDto.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");

            //TODO catch //DbUpdateConcurrencyException into problem

            SemateSazeman semateSazeman = _mapper.Map<SemateSazeman>(semateSazemanDto);
            await _semateSazemanService.Save(semateSazeman);
            return Ok(semateSazeman)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, semateSazeman.Id.ToString()));
        }

        [HttpGet("semate-sazemen")]
        public async Task<ActionResult<IEnumerable<SemateSazemanDto>>> GetAllSemateSazemen(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of SemateSazemen");
            var result = await _semateSazemanService.FindAll(pageable);
            var page = new Page<SemateSazemanDto>(result.Content.Select(entity => _mapper.Map<SemateSazemanDto>(entity)).ToList(),pageable,result.TotalElements);
            return Ok(((IPage<SemateSazemanDto>)page).Content).WithHeaders(page.GeneratePaginationHttpHeaders());
        }

        [HttpGet("semate-sazemen/{id}")]
        public async Task<IActionResult> GetSemateSazeman([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get SemateSazeman : {id}");
            var result = await _semateSazemanService.FindOne(id);
            SemateSazemanDto semateSazemanDto = _mapper.Map<SemateSazemanDto>(result);
            return ActionResultUtil.WrapOrNotFound(semateSazemanDto);
        }

        [HttpDelete("semate-sazemen/{id}")]
        public async Task<IActionResult> DeleteSemateSazeman([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete SemateSazeman : {id}");
            await _semateSazemanService.Delete(id);
            return Ok().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
