using AllianzBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Domain.Base.Entities.PurchaseHistories
{
    public interface IPurchaseHistoryRepository : IRepository<PurchaseHistory>
    {
        Task<(List<PurchaseHistory>, PagingData pagingData)> GetPuchaseHistories(GetPurchaseHistoryRequest request);
    }
}
