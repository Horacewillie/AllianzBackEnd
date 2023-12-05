using AllianzBackEnd.Domain.Base.Entities.PurchaseHistories;
using AllianzBackEnd.Domain.Base.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Infrastructure.Data
{
    internal class UserEntityConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Ignore(g => g.PurchaseHistories);
            builder.HasMany<PurchaseHistory>("_purchaseHistories")
              .WithOne(x => x.User)
              .HasForeignKey(x => x.UserId);
        }
    }
}
