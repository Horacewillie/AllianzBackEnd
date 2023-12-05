using AllianzBackEnd.Domain.Base.Entities.PurchaseHistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Domain.Models
{
    public record GetPurchaseHistoryRequest
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Guid UserId { get; set; }
    }

    public record PurchaseHistoryResponse
    {
        public PagingData PagingData { get; set; }
        public List<PurchaseHistory> PurchaseHistories { get; set; } = new List<PurchaseHistory>();
    }
}
