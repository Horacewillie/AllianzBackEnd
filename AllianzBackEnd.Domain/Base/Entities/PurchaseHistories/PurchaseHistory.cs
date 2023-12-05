using AllianzBackEnd.Domain.Base.Entities.Users;
using AllianzBackEnd.Domain.Enums;
using AllianzBackEnd.Domain.Models;
using System.Text.Json.Serialization;

namespace AllianzBackEnd.Domain.Base.Entities.PurchaseHistories
{
    public class PurchaseHistory : DbGuidEntity
    {
        public decimal Amount { get; private set; }

        public TransactionStatus TransactionStatus { get; private set; }

        public string? TransactionReference { get; set; }

        public string CarMake { get; private set; }

        public string CarModel { get; private set; }

        public string RegNumber { get; set; }

        public Guid? UserId { get; private set; }

        [JsonIgnore]
        public virtual User User { get; private set; }

        protected PurchaseHistory()
            : base()
        {

        }

        public PurchaseHistory(PurchaseRequest request)
        : this()
        {
            CarMake = request.CarMake;
            CarModel = request.CarModel;
            TransactionStatus = TransactionStatus.Pending;
            Amount = request.Amount;
            RegNumber = request.RegNumber;
            UserId = request.UserId;
        }

        public void UpdateTransactionStatus(TransactionStatus status)
        {
            TransactionStatus = status;
        }
    }
}
