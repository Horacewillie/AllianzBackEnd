using AllianzBackEnd.Domain.Base.Entities.PurchaseHistories;
using AllianzBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AllianzBackEnd.Domain.Base.Entities.Users
{
    public class User : DbGuidEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Password { get; set; }

        public string Salt { get; set; }

        protected virtual List<PurchaseHistory> _purchaseHistories { get; set; }

        [JsonIgnore]
        public virtual IReadOnlyCollection<PurchaseHistory> PurchaseHistories => _purchaseHistories.ToList();
        protected User()
            : base()
        {
            _purchaseHistories = new List<PurchaseHistory>();
        }

        public User(CreateUserRequest request)
        : this()
        {
            FirstName = request.FirstName;
            LastName = request.LastName;
            Email = request.Email;
            PhoneNumber = request.PhoneNumber;
        }

        public void AddPurchaseHistory(PurchaseHistory purchaseHistory)
        {
            _purchaseHistories.Add(purchaseHistory);
        }


    }
}
