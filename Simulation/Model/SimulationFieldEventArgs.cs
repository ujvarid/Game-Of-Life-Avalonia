using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Model
{
    public class SimulationFieldEventArgs
    {
        public int X;
        public int Y;
        public bool Value;
        public SimulationFieldEventArgs(int x, int y, bool val)
        {
            X = x;
            Y = y;
            Value = val;
        }
    }
}
