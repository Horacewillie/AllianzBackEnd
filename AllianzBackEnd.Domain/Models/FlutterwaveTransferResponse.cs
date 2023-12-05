using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Domain.Models
{
    public class FlutterwaveTransferResponse
    {
        public string Account_Number { get; set; }
        public string Bank_Code { get; set; }
        public string Full_Name { get; set; }
        public DateTime Created_at { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public int Is_Approved { get; set; }
    }
}
