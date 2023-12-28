using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    internal sealed class TickController : MonoBehaviour, ITickController
    {
        private List<IUpdateController> _updateControllers;

        private Action<IUpdateController> _updateControllerFiniteCallback;

        private void Awake()
        {
            _updateControllers = new List<IUpdateController>(32);

            _updateControllerFiniteCallback = UpdateControllerFinite_Callback;
        }

        private void Update()
        {
            for (int i = _updateControllers.Count - 1; i >= 0; i--)
            {
                _updateControllers[i].UpdateController(Time.deltaTime);
            }
        }

        public IFinite AddController(IUpdateController updateController)
        {
            ControllerHolder<IUpdateController> holder = new ControllerHolder<IUpdateController>(updateController);
            holder.OnControllerFinite += _updateControllerFiniteCallback;
            _updateControllers.Add(updateController);

            return holder;
        }

        private void UpdateControllerFinite_Callback(IUpdateController controller)
        {
            _updateControllers.Remove(controller);
        }
    }
}