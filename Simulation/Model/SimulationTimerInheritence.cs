﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Simulation.Model
{
    public class SimulationTimerInheritence : System.Timers.Timer, ITimer
    {
        private readonly Dictionary<EventHandler, ElapsedEventHandler> _delegateMapper = new();

        // Definiálunk egy Elapsed eseményt és elfedjük vele a System.Timers.Timer-től örökölt azonos nevű eseményt
        public new event EventHandler? Elapsed
        {
            // amikor feliratkoznak az eseményre ...
            add
            {
                if (value != null)
                {
                    var handler = new ElapsedEventHandler(value.Invoke);
                    // egy ElapsedEventHandler-be csomagoljuk az EventHandler-t
                    // (típusbiztos az eseményargumentum típusának kontravarianciája miatt)
                    _delegateMapper.Add(value, handler);
                    // eltároljuk az (EventHandler, ElapsedEventHandler) párost,
                    // erre az eseményről leiratkozás támogatásához van szükség
                    base.Elapsed += handler;
                }
            }
            // amikor leiratkoznak az eseményről ...
            remove
            {
                // előkeressük az EventHandler-hez tartozó ElapsedEventHandler-t
                if (value != null && _delegateMapper.TryGetValue(value, out var handler))
                {
                    _delegateMapper.Remove(value);
                    base.Elapsed -= handler;
                    // leiratkozunk vele
                }
            }
        }
    }
}
