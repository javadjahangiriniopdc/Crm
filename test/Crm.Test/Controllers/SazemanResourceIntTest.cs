
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
    public class SazemanResourceIntTest {
        public SazemanResourceIntTest()
        {
            _factory = new NhipsterWebApplicationFactory<TestStartup>().WithMockUser();
            _client = _factory.CreateClient();

            _sazemanRepository = _factory.GetRequiredService<ISazemanRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            _mapper = config.CreateMapper();

            InitTest();
        }

        private const string DefaultSazemanName = "AAAAAAAAAA";
        private const string UpdatedSazemanName = "BBBBBBBBBB";

        private readonly NhipsterWebApplicationFactory<TestStartup> _factory;
        private readonly HttpClient _client;
        private readonly ISazemanRepository  _sazemanRepository;

        private Sazeman _sazeman;

        private readonly IMapper _mapper;

        private Sazeman CreateEntity()
        {
            return new Sazeman {
                SazemanName = DefaultSazemanName
            };
        }

        private void InitTest()
        {
            _sazeman = CreateEntity();
        }

        [Fact]
        public async Task CreateSazeman()
        {
            var databaseSizeBeforeCreate = await _sazemanRepository.CountAsync();

            // Create the Sazeman
            SazemanDto _sazemanDto = _mapper.Map<SazemanDto>(_sazeman);
            var response = await _client.PostAsync("/api/sazemen", TestUtil.ToJsonContent(_sazemanDto));
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Validate the Sazeman in the database
            var sazemanList = await _sazemanRepository.GetAllAsync();
            sazemanList.Count().Should().Be(databaseSizeBeforeCreate + 1);
            var testSazeman = sazemanList.Last();
            testSazeman.SazemanName.Should().Be(DefaultSazemanName);
        }

        [Fact]
        public async Task CreateSazemanWithExistingId()
        {
            var databaseSizeBeforeCreate = await _sazemanRepository.CountAsync();
            databaseSizeBeforeCreate.Should().Be(0);
            // Create the Sazeman with an existing ID
            _sazeman.Id = 1L;

            // An entity with an existing ID cannot be created, so this API call must fail
            SazemanDto _sazemanDto = _mapper.Map<SazemanDto>(_sazeman);
            var response = await _client.PostAsync("/api/sazemen", TestUtil.ToJsonContent(_sazemanDto));

            // Validate the Sazeman in the database
            var sazemanList = await _sazemanRepository.GetAllAsync();
            sazemanList.Count().Should().Be(databaseSizeBeforeCreate);
        }

        [Fact]
        public async Task CheckSazemanNameIsRequired()
        {
            var databaseSizeBeforeTest = await _sazemanRepository.CountAsync();

            // Set the field to null
            _sazeman.SazemanName = null;

            // Create the Sazeman, which fails.
            SazemanDto _sazemanDto = _mapper.Map<SazemanDto>(_sazeman);
            var response = await _client.PostAsync("/api/sazemen", TestUtil.ToJsonContent(_sazemanDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var sazemanList = await _sazemanRepository.GetAllAsync();
            sazemanList.Count().Should().Be(databaseSizeBeforeTest);
        }

        [Fact]
        public async Task GetAllSazemen()
        {
            // Initialize the database
            await _sazemanRepository.CreateOrUpdateAsync(_sazeman);
            await _sazemanRepository.SaveChangesAsync();

            // Get all the sazemanList
            var response = await _client.GetAsync("/api/sazemen?sort=id,desc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.[*].id").Should().Contain(_sazeman.Id);
            json.SelectTokens("$.[*].sazemanName").Should().Contain(DefaultSazemanName);
        }

        [Fact]
        public async Task GetSazeman()
        {
            // Initialize the database
            await _sazemanRepository.CreateOrUpdateAsync(_sazeman);
            await _sazemanRepository.SaveChangesAsync();

            // Get the sazeman
            var response = await _client.GetAsync($"/api/sazemen/{_sazeman.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.id").Should().Contain(_sazeman.Id);
            json.SelectTokens("$.sazemanName").Should().Contain(DefaultSazemanName);
        }

        [Fact]
        public async Task GetNonExistingSazeman()
        {
            var response = await _client.GetAsync("/api/sazemen/" + long.MaxValue);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateSazeman()
        {
            // Initialize the database
            await _sazemanRepository.CreateOrUpdateAsync(_sazeman);
            await _sazemanRepository.SaveChangesAsync();
            var databaseSizeBeforeUpdate = await _sazemanRepository.CountAsync();

            // Update the sazeman
            var updatedSazeman = await _sazemanRepository.QueryHelper().GetOneAsync(it => it.Id == _sazeman.Id);
            // Disconnect from session so that the updates on updatedSazeman are not directly saved in db
//TODO detach
            updatedSazeman.SazemanName = UpdatedSazemanName;

            SazemanDto updatedSazemanDto = _mapper.Map<SazemanDto>(_sazeman);
            var response = await _client.PutAsync("/api/sazemen", TestUtil.ToJsonContent(updatedSazemanDto));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the Sazeman in the database
            var sazemanList = await _sazemanRepository.GetAllAsync();
            sazemanList.Count().Should().Be(databaseSizeBeforeUpdate);
            var testSazeman = sazemanList.Last();
            testSazeman.SazemanName.Should().Be(UpdatedSazemanName);
        }

        [Fact]
        public async Task UpdateNonExistingSazeman()
        {
            var databaseSizeBeforeUpdate = await _sazemanRepository.CountAsync();

            // If the entity doesn't have an ID, it will throw BadRequestAlertException
            SazemanDto _sazemanDto = _mapper.Map<SazemanDto>(_sazeman);
            var response = await _client.PutAsync("/api/sazemen", TestUtil.ToJsonContent(_sazemanDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Validate the Sazeman in the database
            var sazemanList = await _sazemanRepository.GetAllAsync();
            sazemanList.Count().Should().Be(databaseSizeBeforeUpdate);
        }

        [Fact]
        public async Task DeleteSazeman()
        {
            // Initialize the database
            await _sazemanRepository.CreateOrUpdateAsync(_sazeman);
            await _sazemanRepository.SaveChangesAsync();
            var databaseSizeBeforeDelete = await _sazemanRepository.CountAsync();

            var response = await _client.DeleteAsync($"/api/sazemen/{_sazeman.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the database is empty
            var sazemanList = await _sazemanRepository.GetAllAsync();
            sazemanList.Count().Should().Be(databaseSizeBeforeDelete - 1);
        }

        [Fact]
        public void EqualsVerifier()
        {
            TestUtil.EqualsVerifier(typeof(Sazeman));
            var sazeman1 = new Sazeman {
                Id = 1L
            };
            var sazeman2 = new Sazeman {
                Id = sazeman1.Id
            };
            sazeman1.Should().Be(sazeman2);
            sazeman2.Id = 2L;
            sazeman1.Should().NotBe(sazeman2);
            sazeman1.Id = 0;
            sazeman1.Should().NotBe(sazeman2);
        }
    }
}
