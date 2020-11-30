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
    public class SemateSazemanRepository : GenericRepository<SemateSazeman>, ISemateSazemanRepository
    {
        public SemateSazemanRepository(IUnitOfWork context) : base(context) 
        {
        }

        public override async Task<SemateSazeman> CreateOrUpdateAsync(SemateSazeman semateSazeman)
        {
            bool exists = await Exists(x => x.Id == semateSazeman.Id);

            if (semateSazeman.Id != 0 && exists) {
                Update(semateSazeman);
                /* Force the reference navigation property to be in "modified" state.
                This allows to modify it with a null value (the field is nullable).
                This takes into consideration the case of removing the association between the two instances. */
                _context.SetEntityStateModified(semateSazeman, semateSazeman0 => semateSazeman0.Sazeman);
            } else {
                _context.AddGraph(semateSazeman);
            }

            return semateSazeman;
        }
    }
}
