using Kris.App.Common;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Kris.App.Map
{
    public class Test : ViewModelBase
    {
        public string Text { get; set; }

        public ICommand SwitchTextCommand => new Command(SwitchText);

        public Test()
        {
            Text = "test";
        }

        public void SwitchText()
        {
            if (Text == "test")
            {
                Text = "changed";
            }
            else
            {
                Text = "test";
            }

            OnPropertyChanged(nameof(Text));
        }
    }
}
