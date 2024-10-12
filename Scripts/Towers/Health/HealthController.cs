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
            Eventbus.HealthEvents.OnNewDoubleTower += CreateDoubleHealth;
            Eventbus.HealthEvents.OnHealthsSet += SetHealthHoldersRequest;
        }
        
        private void SetHealthHoldersRequest()
        {
            foreach (var id in ActorHolder.Registry.Keys)
            {
                Eventbus.HealthEvents.OnHealthChange?.Invoke(ActorHolder.Registry[id].Health, id);
            }
        }
        
        void ApplyDamage(int towerID, int damage, Action completeCall)
        {
            ActorHolder.Registry[towerID].Health -= damage; //bug: burda double'a denk gelirse!! double ID girilmiyor çünkü shoot towerlarla ilgili. First towerı shoor et diyebiliriz
            Eventbus.HealthEvents.OnHealthChange?.Invoke(ActorHolder.Registry[towerID].Health, towerID);

            if (IsDead(towerID, completeCall)) return;

            completeCall();
        }

        private void CreateDoubleHealth(int towerID, int[] ids)
        {
            // int totalHealth = 0;
            // foreach (var id in ids)
            // {
            //     totalHealth += ActorHolder.Registry[id].Health;
            //     Holder.RemoveItem(id);
            // }
            
            //Eventbus.HealthEvents.OnDoubleHealthCreated?.Invoke(ids, totalHealth, towerID);
        }

        private bool IsDead(int id, Action completeCall)
        {
            if (ActorHolder.Registry[id].Health <= 0)
            {
                DeathOperator.Instance.HandleDeath(id, 
                    () => Eventbus.CombatEvents.OnTowerKilled?.Invoke(id), 
                    completeCall);

                return true;
            }

            return false;
        }
        
        public override void Unsubscribe()
        {
            Eventbus.HealthEvents.OnShoot -= ApplyDamage;
            Eventbus.HealthEvents.OnNewDoubleTower -= CreateDoubleHealth;
            Eventbus.HealthEvents.OnHealthsSet -= SetHealthHoldersRequest;
        }
        
        // public void SetHealth(int towerID, int newHealth)
        // {
        //     Registry[towerID].Health = newHealth;
        //     Eventbus.HealthEvents.OnHealthChange?.Invoke(Registry[towerID].Health, towerID);
        // }
       
    }
}