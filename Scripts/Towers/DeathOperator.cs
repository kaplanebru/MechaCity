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
            
                StartCoroutine(DeathRoutine(teamSwitchCallback, pairID, ActorHolder.Registry[actorID]));
            
            
        }
        
        
        public IEnumerator DeathRoutine(Action teamSwitchCallback, int pairID, ActorData actor)
        {
            yield return new WaitForSeconds(actor.Towers[0].timingData.shakeDuration);
            yield return new WaitForSeconds(.3f);

            foreach (var tower in actor.Towers)
            {
                MediatorEventbus.EffectEvents.OnDeathEffect?.Invoke(tower.UniqID);
                tower.Mover.RotateMiddle();
            }
           
            teamSwitchCallback.Invoke();

            yield return new WaitForSeconds(actor.Towers[0].timingData.colorFadeDuration);
            Eventbus.CombatEvents.OnCombatCompleteRequest?.Invoke(pairID);
        }
    }
}