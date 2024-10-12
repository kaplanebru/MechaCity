using System;
using System.Collections;
using Actor;
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


        public void HandleDeath(string actorID, Action teamSwitchCallback, Action completeCombat)
        {
            foreach (var tower in ActorHolder.Registry[actorID].Towers)
            {
                StartCoroutine(DeathRoutine(teamSwitchCallback, completeCombat, AllTowers.GetData(tower)));
            }
            
            // if (AllDoubles.TryGetDoubleByID(actorID) != null)
            // {
            //     var doubleTowers = AllDoubles.DoublesByID[actorID].towers.Values;
            //     foreach (var tower in doubleTowers)
            //     {
            //         StartCoroutine(DeathRoutine(teamSwitchCallback, completeCombat, tower));
            //     }
            // }
            // else
            // {
            //     var tower = AllTowers.GetTower(actorID);
            //     StartCoroutine(DeathRoutine(teamSwitchCallback, completeCombat, tower.Data));
            // }
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