using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Domain.Models
{
    public record PurchaseRequest
    {
        public Guid UserId { get; set; }
        public string CarMake { get; set; }

        public string CarModel { get; set; }

        public string SelectedCarBody  { get; set; }

        public decimal Amount { get; set; }

        public string RegNumber { get; set; }
    }
}
