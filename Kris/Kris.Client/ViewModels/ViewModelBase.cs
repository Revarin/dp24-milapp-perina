using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kris.Client.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Source: https://stackoverflow.com/a/32800248
        protected bool SetPropertyValue<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (value == null ? field != null : !value.Equals(field))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

                return true;
            }

            return false;
        }
    }
}
