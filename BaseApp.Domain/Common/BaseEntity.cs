using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Domain.Common
{
    public abstract class BaseEntity : AuditableEntity
    {
        public int Id { get; set; }

        [Timestamp]
        [Column(TypeName = "rowversion")]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
