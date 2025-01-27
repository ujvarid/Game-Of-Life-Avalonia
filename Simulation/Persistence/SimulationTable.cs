using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Persistence
{
    public class SimulationTable
    {
        #region Fields

        private bool[,] _fields = null!;
        private int _tableSize;

        #endregion

        #region Properties

        public bool[,] Fields => _fields;
        public int TableSize => _tableSize;

        #endregion

        #region Ctor

        public SimulationTable(int tablesize)
        {
            _tableSize = tablesize;
            GenerateTable();
        }

        #endregion

        #region Private Game Methods

        private void GenerateTable()
        {
            _fields = new bool[TableSize, TableSize];
            for (int i = 0; i < TableSize; i++)
            {
                for (int j = 0; j < TableSize; j++)
                {
                    _fields[i, j] = false;
                }
            }
        }

        #endregion

        #region Public Game Methods

        public void StepValue(int x, int y, bool value)
        {
            _fields[x, y] = value;
        }

        public void Clear()
        {
            GenerateTable();
        }

        #endregion
    }
}
