using System.ComponentModel.DataAnnotations.Schema;

namespace Kris.Server.Data
{
    [Table("USERS")]
    public class UserEntity : EntityBase<int>
    {
        [Column("IDENTIFICATION")]
        public string Identification { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("CREATED_DATE", TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
    }
}
