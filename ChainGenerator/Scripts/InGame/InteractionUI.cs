using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEngine;
using UnityEngine.UI;

namespace ChainInGame
{
    public class InteractionUI : MonoBehaviour
    {
        private Dropdown _dropdown;
        List<string> _machineryNames = new();
        private void OnEnable()
        {
            _dropdown = GetComponentInChildren<Dropdown>();
            ChainEvents.InGameEvents.OnMachineriesSet += SetupOptions;
        }

        private void SetupOptions(Machinery[] machineries)
        {
            foreach (var machinery in machineries)
            {
                _machineryNames.Add(machinery.name);
            }
            
            _dropdown.ClearOptions();
            _dropdown.AddOptions(_machineryNames);
            //_dropdown.value = 1;
        }

        public void SendNewOption()
        {
            ChainEvents.InGameEvents.OnOptionSet?.Invoke(_dropdown.value);
        }

        private void OnDisable()
        {
            ChainEvents.InGameEvents.OnMachineriesSet -= SetupOptions;
        }
    }
}
