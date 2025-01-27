using Simulation.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Model
{
    public class SimulationModel
    {
        #region Fields

        private SimulationTable _table = null!;
        private bool _isSimulationOn;
        private ITimer _timer = null!;

        #endregion

        #region Properties

        public bool this[int x, int y] => _table.Fields[x, y];
        public bool IsSimulationOn
        {
            get => _isSimulationOn;
            set
            {
                _isSimulationOn = value;
            }
        }
        public int TableSize => _table.TableSize;
        public int Interval => (int)_timer.Interval;

        #endregion

        #region Events

        public event EventHandler<SimulationFieldEventArgs>? FieldChanged;

        #endregion

        #region Ctor

        public SimulationModel(ITimer timer, int tablesize)
        {
            _isSimulationOn = false;
            _table = new SimulationTable(tablesize);
            _timer = timer;
            _timer.Interval = 1000;
            _timer.Elapsed += Timer_Elapsed;
        }

        #endregion

        #region Private Game Methods

        private void OnFieldChanged(int x, int y)
        {
            FieldChanged?.Invoke(this, new(x, y, this[x, y]));
        }

        private int AliveCellCount(int x, int y)
        {
            int count = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i == x && y == j)
                    {
                        continue;
                    }
                    if (i < 0 || j < 0 || i > TableSize - 1 || j > TableSize - 1)
                    {
                        continue;
                    }
                    if (this[i, j]) // ha élő
                    {
                        ++count;
                    }
                }
            }
            return count;
        }


        private void Iteration()
        {
            List<Tuple<int, int>> dead = new List<Tuple<int, int>>();
            List<Tuple<int, int>> born = new List<Tuple<int, int>>();

            for (int i = 0; i < TableSize; ++i)
            {
                for (int j = 0; j < TableSize; ++j)
                {
                    int count = AliveCellCount(i, j);
                    if (count == 0 || count == 1 || count >= 4)
                    {
                        dead.Add(new(i, j));
                    }
                    else if (count == 3 && !(this[i, j]))
                    {
                        born.Add(new(i, j));
                    }
                }
            }

            foreach (var d in dead)
            {
                _table.StepValue(d.Item1, d.Item2, false);
                OnFieldChanged(d.Item1, d.Item2);
            }
            foreach (var b in born)
            {
                _table.StepValue(b.Item1, b.Item2, true);
                OnFieldChanged(b.Item1, b.Item2);
            }
        }

        #endregion

        #region Public Game Methods

        public void StartSimulation()
        {
            _timer.Start();
            _isSimulationOn = true;
        }

        public void StopSimulation()
        {
            _timer?.Stop();
            _isSimulationOn = false;
        }

        public void Step(int x, int y)
        {
            _table.StepValue(x, y, !(this[x, y]));
            OnFieldChanged(x, y);
        }

        public void ChangeInterval(double interval)
        {
            _timer.Interval = interval;
        }

        public void ChangeTableSize(int tablesize)
        {
            _table = new SimulationTable(tablesize);
        }

        public void Clear()
        {
            _table.Clear();
            for (int i = 0; i < TableSize; i++)
            {
                for (int j = 0; j < TableSize; j++)
                {
                    OnFieldChanged(i, j);
                }
            }
        }

        #endregion

        #region Timer Event Handlers

        private void Timer_Elapsed(object? sender, EventArgs e)
        {
            Iteration();
        }

        #endregion
    }
}
