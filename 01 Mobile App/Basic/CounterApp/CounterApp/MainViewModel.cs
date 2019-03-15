using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace CounterApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int _counter1;
        public int Counter1
        {
            get => _counter1;
            set => SetProperty(ref _counter1, value);
        }

        private int _counter2;
        public int Counter2
        {
            get => _counter2;
            set => SetProperty(ref _counter2, value);
        }

        public ICommand IncrementCounterCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            IncrementCounterCommand = new Command<string>(IncrementCounterById);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void IncrementCounterById(string counterId)
        {
            if (counterId == "0")
                Counter1++;
            else if (counterId == "1")
                Counter2++;
        }

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}