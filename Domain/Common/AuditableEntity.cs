using System;

namespace Domain.Common
{
    //TODO: Belkide ekleyen-güncelleyen bilgisi için generic bir tip alınabilir, string vermek çok hoş olmadı gibi.
    public abstract class AuditableEntity
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
