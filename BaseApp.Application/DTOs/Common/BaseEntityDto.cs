namespace BaseApp.Application.DTOs.Common
{
    public class BaseEntityDto : AuditableEntityDto
    {
        public int Id { get; set; }
        public byte[] RowVersion { get; set; } = null;
    }
}
