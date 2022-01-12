using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FasesDaLua.Views
{
    public class WindowBase : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public WindowBase()
        {

        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
