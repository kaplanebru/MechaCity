using System;
using Towers;

namespace Actor
{
    public class HealthController : ActorController
    {
        public HealthController(ActorManager manager) : base(manager) {}
        public override void Subscribe()
        {
            Eventbus.HealthEvents.OnShoot += ApplyDamage;
            Eventbus.HealthEvents.OnNewDoubleHealth += CreateDoubleHealth;
            Eventbus.HealthEvents.OnHealthsSet += SetHealthHoldersRequest;
        }
        
        private void SetHealthHoldersRequest()
        {
            foreach (var id in ActorManager.Registry.Keys)
            {
                Eventbus.HealthEvents.OnHealthChange?.Invoke(ActorManager.Registry[id].Health, id);
            }
        }
        
        void ApplyDamage(int towerID, int damage, Action completeCall)
        {
            ActorManager.Registry[towerID].Health -= damage;
            Eventbus.HealthEvents.OnHealthChange?.Invoke(ActorManager.Registry[towerID].Health, towerID);

            if (IsDead(towerID, completeCall)) return;

            completeCall();
        }

        private void CreateDoubleHealth(int towerID, int[] ids)
        {
            int totalHealth = 0;
            foreach (var id in ids)
            {
                totalHealth += ActorManager.Registry[id].Health;
                _manager.RemoveItem(id);
            }
            
            Eventbus.HealthEvents.OnDoubleHealthCreated?.Invoke(ids, totalHealth, towerID);
            _manager.RegisterItem(towerID, totalHealth);
        }

        private bool IsDead(int id, Action completeCall)
        {
            if (ActorManager.Registry[id].Health <= 0)
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
            Eventbus.HealthEvents.OnNewDoubleHealth -= CreateDoubleHealth;
            Eventbus.HealthEvents.OnHealthsSet -= SetHealthHoldersRequest;
        }
        
        // public void SetHealth(int towerID, int newHealth)
        // {
        //     Registry[towerID].Health = newHealth;
        //     Eventbus.HealthEvents.OnHealthChange?.Invoke(Registry[towerID].Health, towerID);
        // }
       
    }
}