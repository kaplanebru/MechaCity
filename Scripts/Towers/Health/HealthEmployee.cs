using System;
using Towers;
using UnityEngine;

namespace Actor
{
    public class HealthEmployee : ActorEmployee
    {
        public HealthEmployee(ActorHolder holder) : base(holder) {}
        public override void Subscribe()
        {
            Eventbus.HealthEvents.OnShoot += ApplyDamage;
            Eventbus.CombatEvents.OnTeamSwitch += ResetHealth;
        }
        
       
        
        void ApplyDamage(uint actorID, int damage, Action completeCall)
        {
            var actor = ActorHolder.Registry[actorID]; //eski bug: burda double'a denk gelirse!! double ID girilmiyor çünkü shoot towerlarla ilgili. First towerı shoor et diyebiliriz
            var health = actor.Health - damage;
            
            SetHealth(actor, health);
            
            if (IsDead(actorID, completeCall)) return;

            completeCall();
        }

        private bool IsDead(uint actorID, Action completeCall)
        {
            if (ActorHolder.Registry[actorID].Health <= 0)
            {
                DeathOperator.Instance.HandleDeath(actorID, 
                    () => Eventbus.CombatEvents.OnActorKilled?.Invoke(actorID), 
                    completeCall);

                return true;
            }

            return false;
        }

        private void ResetHealth(uint actorID)
        {
            var actor = ActorHolder.Registry[actorID];
            actor.Health = actor.InitialHealth;
            
            Eventbus.HealthEvents.OnHealthChange?.Invoke(actorID);
        }
        
        public void SetHealth(ActorData actor, int health, bool isInitial = false)
        {
            if (isInitial)
                actor.InitialHealth = health;
            actor.Health = health;
            
            Eventbus.HealthEvents.OnHealthChange?.Invoke(actor.ID);
        }
        
        
        public override void Unsubscribe()
        {
            Eventbus.HealthEvents.OnShoot -= ApplyDamage;
            Eventbus.CombatEvents.OnTeamSwitch -= ResetHealth;
        }
        
       
    }
}