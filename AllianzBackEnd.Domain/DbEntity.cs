using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static AllianzBackEnd.Domain.Helpers.DateTimeHelper;

namespace AllianzBackEnd.Domain
{
    public abstract class DbEntityBase
    {
        [Timestamp]
        [JsonIgnore]
        public byte[] RowVersion { get; set; }
    }

    public abstract class DbEntity<T> : DbEntityBase
    {

        public DbEntity()
        {
            CreatedAt = NigerianTime.Now;
            UpdatedAt = NigerianTime.Now;
        }
        public T Id { get; set; }
        [JsonIgnore]
        public bool Deleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public abstract class DbGuidEntity : DbEntity<Guid>
    {
        public DbGuidEntity()
        {
            Id = Guid.NewGuid();

        }
    }
}
