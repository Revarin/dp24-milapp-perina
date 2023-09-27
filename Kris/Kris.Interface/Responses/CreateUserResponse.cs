namespace Kris.Interface
{
    public class CreateUserResponse : ResponseBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
