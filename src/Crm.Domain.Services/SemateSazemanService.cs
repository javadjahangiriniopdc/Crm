using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using Crm.Domain.Services.Interfaces;
using Crm.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Crm.Domain.Services {
    public class SemateSazemanService : ISemateSazemanService {
        protected readonly ISemateSazemanRepository  _semateSazemanRepository;

        public SemateSazemanService(ISemateSazemanRepository semateSazemanRepository)
        {
            _semateSazemanRepository = semateSazemanRepository;
        }

        public virtual async Task<SemateSazeman> Save(SemateSazeman semateSazeman)
        {
            await _semateSazemanRepository.CreateOrUpdateAsync(semateSazeman);
            await _semateSazemanRepository.SaveChangesAsync();
            return semateSazeman;
        }

        public virtual async Task<IPage<SemateSazeman>> FindAll(IPageable pageable)
        {
            var page = await _semateSazemanRepository.QueryHelper()
                .Include(semateSazeman => semateSazeman.Sazeman)
                .GetPageAsync(pageable);
            return page;
        }

        public virtual async Task<SemateSazeman> FindOne(long id)
        {
            var result = await _semateSazemanRepository.QueryHelper()
                .Include(semateSazeman => semateSazeman.Sazeman)
                .GetOneAsync(semateSazeman => semateSazeman.Id == id);
            return result;
        }

        public virtual async Task Delete(long id)
        {
            await _semateSazemanRepository.DeleteByIdAsync(id);
            await _semateSazemanRepository.SaveChangesAsync();
        }
    }
}
