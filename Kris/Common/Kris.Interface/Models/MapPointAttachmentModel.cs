namespace Kris.Interface.Models;

public sealed class MapPointAttachmentModel
{
    public Guid? Id { get; set; }
    public required string Base64Bytes { get; set; }
    public required string FileExtension { get; set; }
    public string? Name { get; set; }
}
