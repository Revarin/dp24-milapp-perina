using System.Runtime.CompilerServices;

namespace Kris.App.Common
{
    public class ViewModelBase : EventToCommandBehavior
    {
        // Source: https://stackoverflow.com/a/32800248
        protected bool SetPropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (value == null ? field != null : !value.Equals(field))
            {
                field = value;
                OnPropertyChanged(propertyName);

                return true;
            }

            return false;
        }
    }

}
