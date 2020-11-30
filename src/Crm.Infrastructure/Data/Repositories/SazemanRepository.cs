using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JHipsterNet.Core.Pagination;
using JHipsterNet.Core.Pagination.Extensions;
using Crm.Domain;
using Crm.Domain.Repositories.Interfaces;
using Crm.Infrastructure.Data.Extensions;

namespace Crm.Infrastructure.Data.Repositories
{
    public class SazemanRepository : GenericRepository<Sazeman>, ISazemanRepository
    {
        public SazemanRepository(IUnitOfWork context) : base(context) 
        {
        }

        public override async Task<Sazeman> CreateOrUpdateAsync(Sazeman sazeman)
        {
            bool exists = await Exists(x => x.Id == sazeman.Id);

            if (sazeman.Id != 0 && exists) {
                Update(sazeman);
            } else {
                _context.AddGraph(sazeman);
            }

            return sazeman;
        }
    }
}
