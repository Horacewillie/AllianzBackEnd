using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Domain.Models
{
    public class FlutterwaveTransactionRequest
    {
        public decimal Amount { get; set; }

        public Guid TransactionId { get; set; }

        public string? TransferUrl { get; set; }

        public string? ProviderApikey { get; set; }
        public string? BeneficiaryAccountNumber { get; set; }

        public string? BeneficiaryBankCode { get; set; }

        public int MaxRetry { get; set; }
    }
}
