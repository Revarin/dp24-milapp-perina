namespace Kris.Server.Data.Models;

// Source: https://learn.microsoft.com/en-us/answers/questions/682240/best-practice-for-saving-image-in-database
public sealed class MapPointAttachmentEntity : EntityBase
{
    public required byte[] Bytes { get; set; }
    public required string FileExtension { get; set; }
    public string? Name { get; set; }
    public required Guid MapPointId { get; set; }
    public MapPointEntity? MapPoint { get; set; }
}
