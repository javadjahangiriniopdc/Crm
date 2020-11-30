using System;

using AutoMapper;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Crm.Infrastructure.Data;
using Crm.Domain;
using Crm.Domain.Repositories.Interfaces;
using Crm.Dto;
using Crm.Configuration.AutoMapper;
using Crm.Test.Setup;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Crm.Test.Controllers {
    public class ContactResourceIntTest {
        public ContactResourceIntTest()
        {
            _factory = new NhipsterWebApplicationFactory<TestStartup>().WithMockUser();
            _client = _factory.CreateClient();

            _contactRepository = _factory.GetRequiredService<IContactRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            _mapper = config.CreateMapper();

            InitTest();
        }

        private const string DefaultPersonCode = "AAAAAAAAAA";
        private const string UpdatedPersonCode = "BBBBBBBBBB";

        private const string DefaultContactName = "AAAAAAAAAA";
        private const string UpdatedContactName = "BBBBBBBBBB";

        private static readonly DateTime? DefaultBirthDate = DateTime.UnixEpoch;
        private static readonly DateTime? UpdatedBirthDate = DateTime.Now;

        private static readonly string DefaultDescription = "jjjjj";
        private static readonly string UpdatedDescription = "jjjjj";

        private static readonly byte[] DefaultAttachFile = null ;
        private static readonly byte[] UpdatedAttachFile = null;

        private readonly NhipsterWebApplicationFactory<TestStartup> _factory;
        private readonly HttpClient _client;
        private readonly IContactRepository  _contactRepository;

        private Contact _contact;

        private readonly IMapper _mapper;

        private Contact CreateEntity()
        {
            return new Contact {
                PersonCode = DefaultPersonCode,
                ContactName = DefaultContactName,
                BirthDate = DefaultBirthDate,
                Description = DefaultDescription,
                AttachFile = DefaultAttachFile
            };
        }

        private void InitTest()
        {
            _contact = CreateEntity();
        }

        [Fact]
        public async Task CreateContact()
        {
            var databaseSizeBeforeCreate = await _contactRepository.CountAsync();

            // Create the Contact
            ContactDto _contactDto = _mapper.Map<ContactDto>(_contact);
            var response = await _client.PostAsync("/api/contacts", TestUtil.ToJsonContent(_contactDto));
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Validate the Contact in the database
            var contactList = await _contactRepository.GetAllAsync();
            contactList.Count().Should().Be(databaseSizeBeforeCreate + 1);
            var testContact = contactList.Last();
            testContact.PersonCode.Should().Be(DefaultPersonCode);
            testContact.ContactName.Should().Be(DefaultContactName);
            testContact.BirthDate.Should().Be(DefaultBirthDate);
            testContact.Description.Should().Be(DefaultDescription);
            testContact.AttachFile.Should().Be(DefaultAttachFile);
        }

        [Fact]
        public async Task CreateContactWithExistingId()
        {
            var databaseSizeBeforeCreate = await _contactRepository.CountAsync();
            databaseSizeBeforeCreate.Should().Be(0);
            // Create the Contact with an existing ID
            _contact.Id = 1L;

            // An entity with an existing ID cannot be created, so this API call must fail
            ContactDto _contactDto = _mapper.Map<ContactDto>(_contact);
            var response = await _client.PostAsync("/api/contacts", TestUtil.ToJsonContent(_contactDto));

            // Validate the Contact in the database
            var contactList = await _contactRepository.GetAllAsync();
            contactList.Count().Should().Be(databaseSizeBeforeCreate);
        }

        [Fact]
        public async Task CheckPersonCodeIsRequired()
        {
            var databaseSizeBeforeTest = await _contactRepository.CountAsync();

            // Set the field to null
            _contact.PersonCode = null;

            // Create the Contact, which fails.
            ContactDto _contactDto = _mapper.Map<ContactDto>(_contact);
            var response = await _client.PostAsync("/api/contacts", TestUtil.ToJsonContent(_contactDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var contactList = await _contactRepository.GetAllAsync();
            contactList.Count().Should().Be(databaseSizeBeforeTest);
        }

        [Fact]
        public async Task CheckContactNameIsRequired()
        {
            var databaseSizeBeforeTest = await _contactRepository.CountAsync();

            // Set the field to null
            _contact.ContactName = null;

            // Create the Contact, which fails.
            ContactDto _contactDto = _mapper.Map<ContactDto>(_contact);
            var response = await _client.PostAsync("/api/contacts", TestUtil.ToJsonContent(_contactDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var contactList = await _contactRepository.GetAllAsync();
            contactList.Count().Should().Be(databaseSizeBeforeTest);
        }

        [Fact]
        public async Task GetAllContacts()
        {
            // Initialize the database
            await _contactRepository.CreateOrUpdateAsync(_contact);
            await _contactRepository.SaveChangesAsync();

            // Get all the contactList
            var response = await _client.GetAsync("/api/contacts?sort=id,desc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.[*].id").Should().Contain(_contact.Id);
            json.SelectTokens("$.[*].personCode").Should().Contain(DefaultPersonCode);
            json.SelectTokens("$.[*].contactName").Should().Contain(DefaultContactName);
            json.SelectTokens("$.[*].birthDate").Should().Contain(DefaultBirthDate);
            json.SelectTokens("$.[*].description").Should().Contain(DefaultDescription);
            json.SelectTokens("$.[*].attachFile").Should().Contain(DefaultAttachFile);
        }

        [Fact]
        public async Task GetContact()
        {
            // Initialize the database
            await _contactRepository.CreateOrUpdateAsync(_contact);
            await _contactRepository.SaveChangesAsync();

            // Get the contact
            var response = await _client.GetAsync($"/api/contacts/{_contact.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.id").Should().Contain(_contact.Id);
            json.SelectTokens("$.personCode").Should().Contain(DefaultPersonCode);
            json.SelectTokens("$.contactName").Should().Contain(DefaultContactName);
            json.SelectTokens("$.birthDate").Should().Contain(DefaultBirthDate);
            json.SelectTokens("$.description").Should().Contain(DefaultDescription);
            json.SelectTokens("$.attachFile").Should().Contain(DefaultAttachFile);
        }

        [Fact]
        public async Task GetNonExistingContact()
        {
            var response = await _client.GetAsync("/api/contacts/" + long.MaxValue);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateContact()
        {
            // Initialize the database
            await _contactRepository.CreateOrUpdateAsync(_contact);
            await _contactRepository.SaveChangesAsync();
            var databaseSizeBeforeUpdate = await _contactRepository.CountAsync();

            // Update the contact
            var updatedContact = await _contactRepository.QueryHelper().GetOneAsync(it => it.Id == _contact.Id);
            // Disconnect from session so that the updates on updatedContact are not directly saved in db
//TODO detach
            updatedContact.PersonCode = UpdatedPersonCode;
            updatedContact.ContactName = UpdatedContactName;
            updatedContact.BirthDate = UpdatedBirthDate;
            updatedContact.Description = UpdatedDescription;
            updatedContact.AttachFile = UpdatedAttachFile;

            ContactDto updatedContactDto = _mapper.Map<ContactDto>(_contact);
            var response = await _client.PutAsync("/api/contacts", TestUtil.ToJsonContent(updatedContactDto));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the Contact in the database
            var contactList = await _contactRepository.GetAllAsync();
            contactList.Count().Should().Be(databaseSizeBeforeUpdate);
            var testContact = contactList.Last();
            testContact.PersonCode.Should().Be(UpdatedPersonCode);
            testContact.ContactName.Should().Be(UpdatedContactName);
            testContact.BirthDate.Should().Be(UpdatedBirthDate);
            testContact.Description.Should().Be(UpdatedDescription);
            testContact.AttachFile.Should().Be(UpdatedAttachFile);
        }

        [Fact]
        public async Task UpdateNonExistingContact()
        {
            var databaseSizeBeforeUpdate = await _contactRepository.CountAsync();

            // If the entity doesn't have an ID, it will throw BadRequestAlertException
            ContactDto _contactDto = _mapper.Map<ContactDto>(_contact);
            var response = await _client.PutAsync("/api/contacts", TestUtil.ToJsonContent(_contactDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Validate the Contact in the database
            var contactList = await _contactRepository.GetAllAsync();
            contactList.Count().Should().Be(databaseSizeBeforeUpdate);
        }

        [Fact]
        public async Task DeleteContact()
        {
            // Initialize the database
            await _contactRepository.CreateOrUpdateAsync(_contact);
            await _contactRepository.SaveChangesAsync();
            var databaseSizeBeforeDelete = await _contactRepository.CountAsync();

            var response = await _client.DeleteAsync($"/api/contacts/{_contact.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the database is empty
            var contactList = await _contactRepository.GetAllAsync();
            contactList.Count().Should().Be(databaseSizeBeforeDelete - 1);
        }

        [Fact]
        public void EqualsVerifier()
        {
            TestUtil.EqualsVerifier(typeof(Contact));
            var contact1 = new Contact {
                Id = 1L
            };
            var contact2 = new Contact {
                Id = contact1.Id
            };
            contact1.Should().Be(contact2);
            contact2.Id = 2L;
            contact1.Should().NotBe(contact2);
            contact1.Id = 0;
            contact1.Should().NotBe(contact2);
        }
    }
}
