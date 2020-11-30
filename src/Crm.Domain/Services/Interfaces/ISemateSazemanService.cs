using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using Crm.Domain;

namespace Crm.Domain.Services.Interfaces
{
    public interface ISemateSazemanService
    {
        Task<SemateSazeman> Save(SemateSazeman semateSazeman);

        Task<IPage<SemateSazeman>> FindAll(IPageable pageable);

        Task<SemateSazeman> FindOne(long id);

        Task Delete(long id);
    }
}
