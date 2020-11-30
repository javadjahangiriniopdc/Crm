
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
    public class SemateSazemanResourceIntTest {
        public SemateSazemanResourceIntTest()
        {
            _factory = new NhipsterWebApplicationFactory<TestStartup>().WithMockUser();
            _client = _factory.CreateClient();

            _semateSazemanRepository = _factory.GetRequiredService<ISemateSazemanRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            _mapper = config.CreateMapper();

            InitTest();
        }

        private const string DefaultSemateSazemanName = "AAAAAAAAAA";
        private const string UpdatedSemateSazemanName = "BBBBBBBBBB";

        private readonly NhipsterWebApplicationFactory<TestStartup> _factory;
        private readonly HttpClient _client;
        private readonly ISemateSazemanRepository  _semateSazemanRepository;

        private SemateSazeman _semateSazeman;

        private readonly IMapper _mapper;

        private SemateSazeman CreateEntity()
        {
            return new SemateSazeman {
                SemateSazemanName = DefaultSemateSazemanName
            };
        }

        private void InitTest()
        {
            _semateSazeman = CreateEntity();
        }

        [Fact]
        public async Task CreateSemateSazeman()
        {
            var databaseSizeBeforeCreate = await _semateSazemanRepository.CountAsync();

            // Create the SemateSazeman
            SemateSazemanDto _semateSazemanDto = _mapper.Map<SemateSazemanDto>(_semateSazeman);
            var response = await _client.PostAsync("/api/semate-sazemen", TestUtil.ToJsonContent(_semateSazemanDto));
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Validate the SemateSazeman in the database
            var semateSazemanList = await _semateSazemanRepository.GetAllAsync();
            semateSazemanList.Count().Should().Be(databaseSizeBeforeCreate + 1);
            var testSemateSazeman = semateSazemanList.Last();
            testSemateSazeman.SemateSazemanName.Should().Be(DefaultSemateSazemanName);
        }

        [Fact]
        public async Task CreateSemateSazemanWithExistingId()
        {
            var databaseSizeBeforeCreate = await _semateSazemanRepository.CountAsync();
            databaseSizeBeforeCreate.Should().Be(0);
            // Create the SemateSazeman with an existing ID
            _semateSazeman.Id = 1L;

            // An entity with an existing ID cannot be created, so this API call must fail
            SemateSazemanDto _semateSazemanDto = _mapper.Map<SemateSazemanDto>(_semateSazeman);
            var response = await _client.PostAsync("/api/semate-sazemen", TestUtil.ToJsonContent(_semateSazemanDto));

            // Validate the SemateSazeman in the database
            var semateSazemanList = await _semateSazemanRepository.GetAllAsync();
            semateSazemanList.Count().Should().Be(databaseSizeBeforeCreate);
        }

        [Fact]
        public async Task CheckSemateSazemanNameIsRequired()
        {
            var databaseSizeBeforeTest = await _semateSazemanRepository.CountAsync();

            // Set the field to null
            _semateSazeman.SemateSazemanName = null;

            // Create the SemateSazeman, which fails.
            SemateSazemanDto _semateSazemanDto = _mapper.Map<SemateSazemanDto>(_semateSazeman);
            var response = await _client.PostAsync("/api/semate-sazemen", TestUtil.ToJsonContent(_semateSazemanDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var semateSazemanList = await _semateSazemanRepository.GetAllAsync();
            semateSazemanList.Count().Should().Be(databaseSizeBeforeTest);
        }

        [Fact]
        public async Task GetAllSemateSazemen()
        {
            // Initialize the database
            await _semateSazemanRepository.CreateOrUpdateAsync(_semateSazeman);
            await _semateSazemanRepository.SaveChangesAsync();

            // Get all the semateSazemanList
            var response = await _client.GetAsync("/api/semate-sazemen?sort=id,desc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.[*].id").Should().Contain(_semateSazeman.Id);
            json.SelectTokens("$.[*].semateSazemanName").Should().Contain(DefaultSemateSazemanName);
        }

        [Fact]
        public async Task GetSemateSazeman()
        {
            // Initialize the database
            await _semateSazemanRepository.CreateOrUpdateAsync(_semateSazeman);
            await _semateSazemanRepository.SaveChangesAsync();

            // Get the semateSazeman
            var response = await _client.GetAsync($"/api/semate-sazemen/{_semateSazeman.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.id").Should().Contain(_semateSazeman.Id);
            json.SelectTokens("$.semateSazemanName").Should().Contain(DefaultSemateSazemanName);
        }

        [Fact]
        public async Task GetNonExistingSemateSazeman()
        {
            var response = await _client.GetAsync("/api/semate-sazemen/" + long.MaxValue);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateSemateSazeman()
        {
            // Initialize the database
            await _semateSazemanRepository.CreateOrUpdateAsync(_semateSazeman);
            await _semateSazemanRepository.SaveChangesAsync();
            var databaseSizeBeforeUpdate = await _semateSazemanRepository.CountAsync();

            // Update the semateSazeman
            var updatedSemateSazeman = await _semateSazemanRepository.QueryHelper().GetOneAsync(it => it.Id == _semateSazeman.Id);
            // Disconnect from session so that the updates on updatedSemateSazeman are not directly saved in db
//TODO detach
            updatedSemateSazeman.SemateSazemanName = UpdatedSemateSazemanName;

            SemateSazemanDto updatedSemateSazemanDto = _mapper.Map<SemateSazemanDto>(_semateSazeman);
            var response = await _client.PutAsync("/api/semate-sazemen", TestUtil.ToJsonContent(updatedSemateSazemanDto));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the SemateSazeman in the database
            var semateSazemanList = await _semateSazemanRepository.GetAllAsync();
            semateSazemanList.Count().Should().Be(databaseSizeBeforeUpdate);
            var testSemateSazeman = semateSazemanList.Last();
            testSemateSazeman.SemateSazemanName.Should().Be(UpdatedSemateSazemanName);
        }

        [Fact]
        public async Task UpdateNonExistingSemateSazeman()
        {
            var databaseSizeBeforeUpdate = await _semateSazemanRepository.CountAsync();

            // If the entity doesn't have an ID, it will throw BadRequestAlertException
            SemateSazemanDto _semateSazemanDto = _mapper.Map<SemateSazemanDto>(_semateSazeman);
            var response = await _client.PutAsync("/api/semate-sazemen", TestUtil.ToJsonContent(_semateSazemanDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Validate the SemateSazeman in the database
            var semateSazemanList = await _semateSazemanRepository.GetAllAsync();
            semateSazemanList.Count().Should().Be(databaseSizeBeforeUpdate);
        }

        [Fact]
        public async Task DeleteSemateSazeman()
        {
            // Initialize the database
            await _semateSazemanRepository.CreateOrUpdateAsync(_semateSazeman);
            await _semateSazemanRepository.SaveChangesAsync();
            var databaseSizeBeforeDelete = await _semateSazemanRepository.CountAsync();

            var response = await _client.DeleteAsync($"/api/semate-sazemen/{_semateSazeman.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the database is empty
            var semateSazemanList = await _semateSazemanRepository.GetAllAsync();
            semateSazemanList.Count().Should().Be(databaseSizeBeforeDelete - 1);
        }

        [Fact]
        public void EqualsVerifier()
        {
            TestUtil.EqualsVerifier(typeof(SemateSazeman));
            var semateSazeman1 = new SemateSazeman {
                Id = 1L
            };
            var semateSazeman2 = new SemateSazeman {
                Id = semateSazeman1.Id
            };
            semateSazeman1.Should().Be(semateSazeman2);
            semateSazeman2.Id = 2L;
            semateSazeman1.Should().NotBe(semateSazeman2);
            semateSazeman1.Id = 0;
            semateSazeman1.Should().NotBe(semateSazeman2);
        }
    }
}
