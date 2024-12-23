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


        public void HandleDeath(uint actorID, Action teamSwitchCallback, int pairID)
        {
            
                StartCoroutine(DeathRoutine(teamSwitchCallback, pairID, ActorDB.Registry[actorID]));
            
            
        }
        
        
        public IEnumerator DeathRoutine(Action teamSwitchCallback, int pairID, ActorData actor)
        {
            yield return new WaitForSeconds(actor.Towers[0].VisualData.timingData.shakeDuration);
            yield return new WaitForSeconds(.3f);

            foreach (var tower in actor.Towers)
            {
                MediatorEventbus.EffectEvents.OnDeathEffect?.Invoke(tower.NumericData.UniqID);
                tower.VisualData.Mover.RotateMiddle();
            }
           
            teamSwitchCallback.Invoke();

            yield return new WaitForSeconds(actor.Towers[0].VisualData.timingData.colorFadeDuration);
            Eventbus.CombatEvents.OnCombatCompleteRequest?.Invoke(pairID);
        }
    }
}