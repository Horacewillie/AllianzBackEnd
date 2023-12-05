using AllianzBackEnd.Domain.Base.Entities.PurchaseHistories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Infrastructure.Data
{
    internal class PurchaseHistoryEntityConfig : IEntityTypeConfiguration<PurchaseHistory>
    {
        public void Configure(EntityTypeBuilder<PurchaseHistory> builder)
        {
            builder.Navigation(f => f.User).AutoInclude(false);
        }
    }
}
