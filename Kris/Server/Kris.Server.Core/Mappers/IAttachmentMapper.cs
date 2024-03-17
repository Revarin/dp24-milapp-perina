using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public interface IAttachmentMapper
{
    MapPointAttachmentEntity MapPointAttachment(MapPointAttachmentModel model, Guid mapPointId);
}
