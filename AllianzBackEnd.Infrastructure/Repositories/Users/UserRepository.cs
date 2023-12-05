using AllianzBackEnd.Domain.Base.Entities.PurchaseHistories;
using AllianzBackEnd.Domain.Base.Entities.Users;
using AllianzBackEnd.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Infrastructure.Repositories.Users
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context)
        {
        }

        public PurchaseHistory AddPurchaseHistory(PurchaseHistory purchaseHistory)
        {
            var history = DbContext.Set<PurchaseHistory>().Add(purchaseHistory).Entity;

            return history;
        }

        public async Task<User> FindUserByEmailAddress(string emailAddress)
        {
            var user = await DbContext.Users.Where(x => x.Email == emailAddress)
                 .SingleOrDefaultAsync();
            return user;
        }

        public Task<User> RegisterUser(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
