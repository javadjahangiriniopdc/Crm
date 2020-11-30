
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
    public class ContactController : ControllerBase {
        private const string EntityName = "contact";
        private readonly IMapper _mapper;
        private readonly IContactService _contactService;
        private readonly ILogger<ContactController> _log;

        public ContactController(ILogger<ContactController> log,
            IMapper mapper,
            IContactService contactService)
        {
            _log = log;
            _mapper = mapper;
            _contactService = contactService;
        }

        [HttpPost("contacts")]
        [ValidateModel]
        public async Task<ActionResult<ContactDto>> CreateContact([FromBody] ContactDto contactDto)
        {
            _log.LogDebug($"REST request to save Contact : {contactDto}");
            if (contactDto.Id != 0)
                throw new BadRequestAlertException("A new contact cannot already have an ID", EntityName, "idexists");

            Contact contact = _mapper.Map<Contact>(contactDto);
            await _contactService.Save(contact);
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, contact.Id.ToString()));
        }

        [HttpPut("contacts")]
        [ValidateModel]
        public async Task<IActionResult> UpdateContact([FromBody] ContactDto contactDto)
        {
            _log.LogDebug($"REST request to update Contact : {contactDto}");
            if (contactDto.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");

            //TODO catch //DbUpdateConcurrencyException into problem

            Contact contact = _mapper.Map<Contact>(contactDto);
            await _contactService.Save(contact);
            return Ok(contact)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, contact.Id.ToString()));
        }

        [HttpGet("contacts")]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetAllContacts(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of Contacts");
            var result = await _contactService.FindAll(pageable);
            var page = new Page<ContactDto>(result.Content.Select(entity => _mapper.Map<ContactDto>(entity)).ToList(),pageable,result.TotalElements);
            return Ok(((IPage<ContactDto>)page).Content).WithHeaders(page.GeneratePaginationHttpHeaders());
        }

        [HttpGet("contacts/{id}")]
        public async Task<IActionResult> GetContact([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get Contact : {id}");
            var result = await _contactService.FindOne(id);
            ContactDto contactDto = _mapper.Map<ContactDto>(result);
            return ActionResultUtil.WrapOrNotFound(contactDto);
        }

        [HttpDelete("contacts/{id}")]
        public async Task<IActionResult> DeleteContact([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete Contact : {id}");
            await _contactService.Delete(id);
            return Ok().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
