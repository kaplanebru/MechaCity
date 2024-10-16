using System;
using Towers;

namespace Actor
{
    public class HealthController : ActorController
    {
        public HealthController(ActorHolder holder) : base(holder) {}
        public override void Subscribe()
        {
            Eventbus.HealthEvents.OnShoot += ApplyDamage;
            Eventbus.HealthEvents.OnTowersSet += SetHealthHoldersRequest;
            Eventbus.CombatEvents.OnTeamSwitch += ResetHealth;
        }
        
        private void SetHealthHoldersRequest()
        {
            foreach (var actorID in ActorHolder.Registry.Keys)
            {
                Eventbus.HealthEvents.OnHealthChange?.Invoke(actorID); //todo: ui
            }
        }
        
        void ApplyDamage(uint actorID, int damage, Action completeCall)
        {
            ActorHolder.Registry[actorID].Health -= damage; //eski bug: burda double'a denk gelirse!! double ID girilmiyor çünkü shoot towerlarla ilgili. First towerı shoor et diyebiliriz
            Eventbus.HealthEvents.OnHealthChange?.Invoke(actorID); //TODO: ui

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
        
        public override void Unsubscribe()
        {
            Eventbus.HealthEvents.OnShoot -= ApplyDamage;
            Eventbus.HealthEvents.OnTowersSet -= SetHealthHoldersRequest;
            Eventbus.CombatEvents.OnTeamSwitch -= ResetHealth;
        }
        
       
    }
}