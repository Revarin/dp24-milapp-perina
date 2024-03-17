namespace Kris.Client.Core.Mappers;

public sealed class MapObjectsMapper : IMapObjectsMapper
{
    public Models.MapPointListModel MapPoint(Interface.Models.MapPointListModel model)
    {
        return new Models.MapPointListModel
        {
            Id = model.Id,
            Name = model.Name,
            Created = model.Created,
            Creator = new Models.UserListModel
            {
                Id = model.User.Id,
                Name = model.User.Name
            },
            Location = new Location(model.Position.Latitude, model.Position.Longitude, model.Position.Altitude),
            Symbol = model.Symbol
        };
    }

    public Models.MapPointDetailModel MapPoint(Interface.Models.MapPointDetailModel model)
    {
        return new Models.MapPointDetailModel
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Created = model.Created,
            Creator = new Models.UserListModel
            {
                Id = model.User.Id,
                Name = model.User.Name
            },
            Location = new Location(model.Position.Latitude, model.Position.Longitude, model.Position.Altitude),
            Symbol = model.Symbol,
            Attachments = model.Attachments.Select(attachment => new Models.AttachmentModel
            {
                Id = attachment.Id.Value,
                Name = attachment.Name,
                FileExtension = attachment.FileExtension,
                Base64Bytes = attachment.Base64Bytes
            }).ToList()
        };
    }
}
