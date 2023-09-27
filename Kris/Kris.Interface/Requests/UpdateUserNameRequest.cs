namespace Kris.Interface
{
    public class UpdateUserNameRequest : RequestBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
