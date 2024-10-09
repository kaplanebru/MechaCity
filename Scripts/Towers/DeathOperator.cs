using System;
using System.Collections;
using UnityEngine;

namespace Towers
{
    public class DeathOperator: MonoBehaviour
    {
        public static DeathOperator Instance;
        private void Awake()
        {
            Instance = this;
        }
        // public TowerData[] Towers;
        //
        // public void Setup(TowerData[] towers)
        // {
        //     Towers = towers;
        // }


        public void HandleDeath(TowerData[] towers, Action teamSwitchCallback, Action completeCombat)
        {
            foreach (var tower in towers)
            {
                StartCoroutine(DeathRoutine(teamSwitchCallback, completeCombat, tower));
            }
        }
        
        public IEnumerator DeathRoutine(Action teamSwitchCallback, Action completeCombat, TowerData tower)
        {
            yield return new WaitForSeconds(tower.timingData.shakeDuration);

            yield return new WaitForSeconds(.3f);

            MediatorEventbus.EffectEvents.OnDeathEffect?.Invoke(tower.UniqID);
            tower.Mover.RotateMiddle();
            teamSwitchCallback.Invoke();

            yield return new WaitForSeconds(tower.timingData.colorFadeDuration);

            completeCombat.Invoke();
        }
    }
}