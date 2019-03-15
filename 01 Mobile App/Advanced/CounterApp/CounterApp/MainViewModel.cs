using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace CounterApp
{
    public class MainViewModel
    {
        public List<CounterModel> Counters { get; }

        public MainViewModel()
        {
            Counters = new List<CounterModel>
            {
                new CounterModel(1),
                new CounterModel(2)
            };
        }
    }
}