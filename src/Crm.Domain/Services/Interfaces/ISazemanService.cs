using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using Crm.Domain;

namespace Crm.Domain.Services.Interfaces
{
    public interface ISazemanService
    {
        Task<Sazeman> Save(Sazeman sazeman);

        Task<IPage<Sazeman>> FindAll(IPageable pageable);

        Task<Sazeman> FindOne(long id);

        Task Delete(long id);
    }
}
