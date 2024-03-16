using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class AttachmentMapper : IAttachmentMapper
{
    public MapPointAttachmentEntity MapPointAttachment(MapPointAttachmentModel model, Guid mapPointId)
    {
        return new MapPointAttachmentEntity
        {
            Bytes = Convert.FromBase64String(model.Base64Bytes),
            FileExtension = model.FileExtension,
            Name = model.Name,
            MapPointId = mapPointId
        };
    }
}
