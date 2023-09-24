namespace Kris.Server.Data
{
    public class UserEntity : EntityBase<int>
    {
        public string Identification { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
