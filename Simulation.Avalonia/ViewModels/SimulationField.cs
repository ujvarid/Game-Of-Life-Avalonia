using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Avalonia.ViewModels
{
    public class SimulationField : ViewModelBase
    {
        public int X;
        public int Y;
        private bool _alive = false;
        public bool Alive
        {
            get => _alive;
            set
            {
                _alive = value;
                OnPropertyChanged();
            }
        }
        public Tuple<int, int> XY => new(X, Y);
        public RelayCommand<Tuple<int, int>>? StepCommand { get; set; }
    }
}
