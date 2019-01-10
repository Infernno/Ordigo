using Ordigo.Server.Core.Contracts;
using Ordigo.Server.Core.Data.Contexts;
using Ordigo.Server.Core.Data.Entities;

namespace Ordigo.Server.Core.Data.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext mContext) : base(mContext)
        {
        }
    }
}
