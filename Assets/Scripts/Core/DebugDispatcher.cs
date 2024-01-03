using System;
using System.Collections.Generic;

namespace Core
{
    public class DebugDispatcher : IUpdateController
    {
        private List<DispatchUnit> _dispatchQueue;
        
        public DebugDispatcher(ITickController tickController)
        {
            _dispatchQueue = new List<DispatchUnit>();
            tickController.AddController(this);
        }
        
        private class DispatchUnit
        {
            public float Time;
            public Action Action;
        }

        public void Enqueue(Action action)
        {
            _dispatchQueue.Add(new DispatchUnit()
            {
                Time = 0.1f,
                Action = action
            });
        }

        public void UpdateController(float deltaTime)
        {
            for (int i = _dispatchQueue.Count - 1; i >= 0; i--)
            {
                DispatchUnit unit = _dispatchQueue[i];
                unit.Time -= deltaTime;
                if (unit.Time <= 0f)
                {
                    unit.Action.Invoke();
                    _dispatchQueue.Remove(unit);
                }
            }
        }
    }
}