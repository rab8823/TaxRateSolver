using System.ComponentModel;

namespace ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected internal void NotifyPropertyValueChanged(string name)
        {
            var localHandler = PropertyChanged;
            if (localHandler != null)
            {
                localHandler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
