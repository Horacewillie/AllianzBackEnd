using AllianzBackEnd.Domain.Base.Entities.PurchaseHistories;
using AllianzBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Domain.Base.Entities.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindUserByEmailAddress(string emailAddress);

        PurchaseHistory AddPurchaseHistory(PurchaseHistory purchaseHistory);
    }
}
