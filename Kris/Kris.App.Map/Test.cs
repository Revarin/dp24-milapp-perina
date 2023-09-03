using Kris.App.Common;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Kris.App.Map
{
    public class Test : ViewModelBase
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetPropertyValue(ref _text, value); }
        }

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
        }
    }
}
