using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using Crm.Domain;

namespace Crm.Domain.Services.Interfaces
{
    public interface IContactService
    {
        Task<Contact> Save(Contact contact);

        Task<IPage<Contact>> FindAll(IPageable pageable);

        Task<Contact> FindOne(long id);

        Task Delete(long id);
    }
}
