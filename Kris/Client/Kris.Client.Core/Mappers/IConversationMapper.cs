﻿using InterfaceConversationListModel = Kris.Interface.Models.ConversationListModel;
using ClientConversationListModel = Kris.Client.Core.Models.ConversationModel;

namespace Kris.Client.Core.Mappers;

public interface IConversationMapper
{
    ClientConversationListModel Map(InterfaceConversationListModel model);
}
