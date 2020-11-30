using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using Crm.Domain.Services.Interfaces;
using Crm.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Crm.Domain.Services {
    public class SazemanService : ISazemanService {
        protected readonly ISazemanRepository  _sazemanRepository;

        public SazemanService(ISazemanRepository sazemanRepository)
        {
            _sazemanRepository = sazemanRepository;
        }

        public virtual async Task<Sazeman> Save(Sazeman sazeman)
        {
            await _sazemanRepository.CreateOrUpdateAsync(sazeman);
            await _sazemanRepository.SaveChangesAsync();
            return sazeman;
        }

        public virtual async Task<IPage<Sazeman>> FindAll(IPageable pageable)
        {
            var page = await _sazemanRepository.QueryHelper()
                .GetPageAsync(pageable);
            return page;
        }

        public virtual async Task<Sazeman> FindOne(long id)
        {
            var result = await _sazemanRepository.QueryHelper()
                .GetOneAsync(sazeman => sazeman.Id == id);
            return result;
        }

        public virtual async Task Delete(long id)
        {
            await _sazemanRepository.DeleteByIdAsync(id);
            await _sazemanRepository.SaveChangesAsync();
        }
    }
}
