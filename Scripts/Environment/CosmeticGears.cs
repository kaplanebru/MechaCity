using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
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
        }
    }
}

