namespace Kris.Client.Core.Models;

public sealed class AttachmentModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Base64Bytes { get; set; }
    public string FileExtension { get; set; }
}
