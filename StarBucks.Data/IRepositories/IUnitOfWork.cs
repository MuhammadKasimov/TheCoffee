using StarBucks.Domain.Entities;
using System.Threading.Tasks;

namespace StarBucks.Data.IRepositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<Coffee> Coffees { get; }
        IGenericRepository<Attachment> Attachments { get; }
        IGenericRepository<Address> Addresses { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<Orders> Orders { get; }
        ValueTask SaveChangesAsync();
    }
}
