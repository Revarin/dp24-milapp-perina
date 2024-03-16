using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class MapObjectMapper : IMapObjectMapper
{
    public MapPointDetailModel MapPointDetail(MapPointEntity entity)
    {
        return new MapPointDetailModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type,
            Description = entity.Description,
            Created = entity.Created,
            Position = entity.Position,
            Symbol = entity.Symbol,
            User = new UserModel
            {
                Id = entity.SessionUser!.UserId,
                Name = entity.SessionUser!.User?.Login
            },
            Attachments = entity.Attachments.Select(attachment => new MapPointAttachmentModel
            {
                Id = attachment.Id,
                Name = attachment.Name,
                FileExtension = attachment.FileExtension,
                Base64Bytes = Convert.ToBase64String(attachment.Bytes)
            }).ToList()
        };
    }

    public MapPointListModel MapPointList(MapPointEntity entity)
    {
        return new MapPointListModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type,
            Created = entity.Created,
            Position = entity.Position,
            Symbol = entity.Symbol,
            User = new UserModel
            {
                Id = entity.SessionUser!.UserId,
                Name = entity.SessionUser!.User?.Login
            }
        };
    }
}
