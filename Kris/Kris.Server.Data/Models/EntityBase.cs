using System.ComponentModel.DataAnnotations.Schema;

namespace Kris.Server.Data
{
    public class EntityBase<T>
    {
        [Column("ID")]
        public T Id { get; set; }
    }
}
