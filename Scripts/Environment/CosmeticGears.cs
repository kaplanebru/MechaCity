using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using Network;
using UnityEngine;

namespace Environment
{
    public class CosmeticGears : MonoBehaviour
    {
        public Machinery[] machineries;

        private void OnEnable()
        {
            Eventbus.CombatEvents.OnCombatStarted += MoveGears;
            Eventbus.CombatEvents.OnCombatEnding += StopGears;

            NetworkEventbus.OnAllClientsSet += FirstMotion;
        }

        private void FirstMotion(object[] obj)
        {
            MoveGears();
            Invoke(nameof(StopGears), 1.2f);
        }

        private void StopGears()
        {
            foreach (var machinery in machineries)
            {
                machinery.StopMovers();
            }
        }

        private void MoveGears()
        {
            foreach (var machinery in machineries)
            {
                machinery.Move();
            }
        }

        private void OnDisable()
        {
            Eventbus.CombatEvents.OnCombatStarted -= MoveGears;
            Eventbus.CombatEvents.OnCombatEnding -= StopGears;
            NetworkEventbus.OnAllClientsSet -= FirstMotion;

        }
    }
}

