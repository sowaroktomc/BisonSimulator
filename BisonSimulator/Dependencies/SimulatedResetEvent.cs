using System;
using Sowalabs.Bison.LiquidityEngine.Dependencies;
using System.Threading;
using Sowalabs.Bison.ProfitSim.Events;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    internal class SimulatedResetEvent : IResetEvent
    {
        private readonly AutoResetEvent _mainEvent = new AutoResetEvent(false);
        private AutoResetEvent _blockSimulationEvent = new AutoResetEvent(false);
        private readonly SimulationEngine _engine;
        private bool _isEventSet;
        private SetSimulatedResetEventEvent _simulationEvent;
        private BlockSimulationEvent _blockSimulation;

        public SimulatedResetEvent(SimulationEngine engine)
        {
            _engine = engine;
            _blockSimulation = new BlockSimulationEvent(this, _engine);
            _engine.AddEvent(_blockSimulation);
        }

        public void Set()
        {
            _isEventSet = true;
            if (_blockSimulation == null)
            {
                _blockSimulation = new BlockSimulationEvent(this, _engine);
                _engine.AddEvent(_blockSimulation);
            }
            else
            {
                _blockSimulationEvent.Reset();
            }

            if (_simulationEvent != null)
            {
                _engine.CancelEvent(_simulationEvent);
                _simulationEvent = null;
            }

            _mainEvent.Set();
        }

        /// <summary>
        /// Sets event and waits until thread, waiting for the event, is done. Then continues simulation.
        /// </summary>
        internal void SetEvent()
        {
            _simulationEvent = null;
            _mainEvent.Set();
            _blockSimulationEvent?.WaitOne();
        }

        internal void Block()
        {
            _blockSimulation = null;
            _blockSimulationEvent?.WaitOne();
        }

        public bool Wait(int millisecondsTimeout)
        {
            if (millisecondsTimeout < 0)
            {
                throw new NotSupportedException();
            }


            _isEventSet = false;
            _simulationEvent = new SetSimulatedResetEventEvent(this, _engine.CurrentTime.AddMilliseconds(millisecondsTimeout));
            _engine.AddEvent(_simulationEvent);
            _blockSimulationEvent?.Set(); // continue simulation.
            _mainEvent.WaitOne();
            return _isEventSet;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _blockSimulationEvent.Set();
            _blockSimulationEvent.Dispose();
            _blockSimulationEvent = null;

            _mainEvent.Dispose();
        }
    }
}
