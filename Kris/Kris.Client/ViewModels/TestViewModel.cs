using System.Windows.Input;

namespace Kris.Client.ViewModels
{
    public class TestViewModel : ViewModelBase
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetPropertyValue(ref _text, value); }
        }

        public ICommand SwitchTextCommand { get; private set; }

        public TestViewModel()
        {
            Text = "test";
            SwitchTextCommand = new Command(SwitchText);
        }

        private void SwitchText()
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
