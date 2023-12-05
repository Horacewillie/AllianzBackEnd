using AllianzBackEnd.Domain.Base.Entities.PurchaseHistories;
using AllianzBackEnd.Domain.Base.Entities.Users;
using AllianzBackEnd.Domain.Models;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AllianzBackEnd.Infrastructure.Repositories.PurchaseHistories
{
    public class PurchaseHistoryRepository : BaseRepository<PurchaseHistory>, IPurchaseHistoryRepository
    {
        public PurchaseHistoryRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<(List<PurchaseHistory>, PagingData pagingData)> GetPuchaseHistories(GetPurchaseHistoryRequest request)
        {
            IQueryable<PurchaseHistory> purchaseHistoryQuery = DbContext.PurchaseHistories.
               Where(x => x.UserId == request.UserId);

            var (dbSet, pagingData) = purchaseHistoryQuery.Paginate(g => g.CreatedAt, page: request.PageNumber, pageSize: request.PageSize, orderByDescending: true);

            return (await dbSet.ToListAsync(), pagingData);
        }
    }
}
