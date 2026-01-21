using BaseApp.Application.Common.Attributes;

namespace BaseApp.Application.DTOs.Common
{
    public class BaseEntityDto : AuditableEntityDto
    {
        public int Id { get; set; }
        [AutoBindRowVersion]
        public byte[] RowVersion { get; set; } = null;
    }
}
