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
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(IUnitOfWork context) : base(context) 
        {
        }

        public override async Task<Contact> CreateOrUpdateAsync(Contact contact)
        {
            bool exists = await Exists(x => x.Id == contact.Id);

            if (contact.Id != 0 && exists) {
                Update(contact);
                /* Force the reference navigation property to be in "modified" state.
                This allows to modify it with a null value (the field is nullable).
                This takes into consideration the case of removing the association between the two instances. */
               // _context.SetEntityStateModified(contact, contact0 => contact0.Sazeman);
                _context.SetEntityStateModified(contact, contact0 => contact0.SemateSazeman);
            } else {
                _context.AddGraph(contact);
            }

            return contact;
        }
    }
}
