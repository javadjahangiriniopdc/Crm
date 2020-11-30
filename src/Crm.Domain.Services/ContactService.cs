using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using Crm.Domain.Services.Interfaces;
using Crm.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Crm.Domain.Services {
    public class ContactService : IContactService {
        protected readonly IContactRepository  _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public virtual async Task<Contact> Save(Contact contact)
        {
            await _contactRepository.CreateOrUpdateAsync(contact);
            await _contactRepository.SaveChangesAsync();
            return contact;
        }

        public virtual async Task<IPage<Contact>> FindAll(IPageable pageable)
        {
            var page = await _contactRepository.QueryHelper()
                // .Include(contact => contact.Sazeman)
                .Include(contact => contact.SemateSazeman)
                .GetPageAsync(pageable);
            return page;
        }

        public virtual async Task<Contact> FindOne(long id)
        {
            var result = await _contactRepository.QueryHelper()
                // .Include(contact => contact.Sazeman)
                .Include(contact => contact.SemateSazeman)
                .GetOneAsync(contact => contact.Id == id);
            return result;
        }

        public virtual async Task Delete(long id)
        {
            await _contactRepository.DeleteByIdAsync(id);
            await _contactRepository.SaveChangesAsync();
        }
    }
}
