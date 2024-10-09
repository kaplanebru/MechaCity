using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace Health
{
    public class HealthManager
    {
        private Dictionary<int, HealthData> Registry = new(); // TowerID -> Health
        public int GetHealth(int towerID) => Registry[towerID].Health;

        public void Subscribe()
        {
            FillRegistry();

            Eventbus.HealthEvents.OnShoot += ApplyDamage;
            Eventbus.HealthEvents.OnNewDoubleHealth += CreateDoubleHealth;
        }

        void FillRegistry()
        {
            foreach (var tower in AllTowers.Towers)
            {
                var id = tower.Data.UniqID;
                RegisterItem(id, tower.ConstantData.StartHealth);
                Eventbus.HealthEvents.OnHealthChange?.Invoke(Registry[id].Health, id); //possible bug: health holderların yaratılma sırası

            } //todo: double da register edilebilir
        }

        public void RegisterItem(int towerID, int initialHealth)
        {
            if (Registry.ContainsKey(towerID)) return;

            Registry[towerID] = new HealthData(initialHealth);
        }

        private void RemoveItem(int towerID)
        {
            Registry.Remove(towerID);
            Eventbus.HealthEvents.OnRemoveFromRegistry?.Invoke(towerID);
        }


        void ApplyDamage(int towerID, int damage, Action completeCall)
        {
            Registry[towerID].Health -= damage;
            Eventbus.HealthEvents.OnHealthChange?.Invoke(Registry[towerID].Health, towerID);

            if (IsDead(towerID, completeCall)) return;

            completeCall();
        }

        private void CreateDoubleHealth(int towerID, int[] ids)
        {
            int totalHealth = 0;
            foreach (var id in ids)
            {
                totalHealth += Registry[id].Health;
                RemoveItem(id);
            }
            
            Eventbus.HealthEvents.OnCommonHealthIconRequest?.Invoke(ids, totalHealth, towerID);
            RegisterItem(towerID, totalHealth);
        }

        private bool IsDead(int id, Action completeCall)
        {
            if (Registry[id].Health <= 0)
            {
                Debug.Log("dead");
                // DeathOperator.Instance.HandleDeath(healthy.HealthSubjects, 
                //     () => Eventbus.CombatEvents.OnTowerKilled?.Invoke(healthy.HealthID), 
                //     completeCall);

                return true;
            }

            return false;
        }

        public void Unsubscribe()
        {
            Registry.Clear();
            Eventbus.HealthEvents.OnShoot -= ApplyDamage;
            Eventbus.HealthEvents.OnNewDoubleHealth -= CreateDoubleHealth;
        }

        // public void SetHealth(int towerID, int newHealth)
        // {
        //     Registry[towerID].Health = newHealth;
        //     Eventbus.HealthEvents.OnHealthChange?.Invoke(Registry[towerID].Health, towerID);
        // }
    }
}