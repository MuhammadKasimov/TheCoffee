using StarBucks.Data.Contexts;
using StarBucks.Data.IRepositories;
using StarBucks.Domain.Entities;
using System.Threading.Tasks;

namespace StarBucks.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StarbucksDbContext dbContext;

        public UnitOfWork(StarbucksDbContext dbContext)
        {
            this.dbContext = dbContext;
            Coffees = new GenericRepository<Coffee>(dbContext);
            Attachments = new GenericRepository<Attachment>(dbContext);
            Addresses = new GenericRepository<Address>(dbContext);
            Users = new GenericRepository<User>(dbContext);
            Orders = new GenericRepository<Orders>(dbContext);
        }

        public IGenericRepository<Coffee> Coffees { get; }
        public IGenericRepository<Attachment> Attachments { get; }
        public IGenericRepository<Address> Addresses { get; }
        public IGenericRepository<User> Users { get; }
        public IGenericRepository<Orders> Orders { get; }



        public async ValueTask SaveChangesAsync() =>
            await dbContext.SaveChangesAsync();
    }
}
