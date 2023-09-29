using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Kris.Client.Core
{
    public class AppInitializedMessage : ValueChangedMessage<object>
    {
        public AppInitializedMessage(object value) : base(value)
        {
        }
    }
}
