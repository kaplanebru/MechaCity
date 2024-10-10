using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace Health
{
    public class HealthManager
    {
        private static Dictionary<int, HealthData> Registry = new(); // TowerID -> Health
        public static int GetHealth(int towerID) => Registry[towerID].Health;

        public void Subscribe()
        {
            Eventbus.HealthEvents.OnShoot += ApplyDamage;
            Eventbus.HealthEvents.OnNewDoubleHealth += CreateDoubleHealth;
            Eventbus.HealthEvents.OnHealthsSet += SetHealthHoldersRequest;
        }

        private void SetHealthHoldersRequest()
        {
            foreach (var id in Registry.Keys)
            {
                Eventbus.HealthEvents.OnHealthChange?.Invoke(Registry[id].Health, id);
            }
        }

        public void FillRegistry()
        {
            foreach (var tower in AllTowers.Towers)
            {
                var id = tower.Data.UniqID;
                RegisterItem(id, tower.ConstantData.StartHealth);

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
            
            Eventbus.HealthEvents.OnDoubleHealthCreated?.Invoke(ids, totalHealth, towerID);
            RegisterItem(towerID, totalHealth);
        }

        private bool IsDead(int id, Action completeCall)
        {
            if (Registry[id].Health <= 0)
            {
                DeathOperator.Instance.HandleDeath(id, 
                    () => Eventbus.CombatEvents.OnTowerKilled?.Invoke(id), 
                    completeCall);

                return true;
            }

            return false;
        }

        public void Unsubscribe()
        {
            Eventbus.HealthEvents.OnShoot -= ApplyDamage;
            Eventbus.HealthEvents.OnNewDoubleHealth -= CreateDoubleHealth;
            Eventbus.HealthEvents.OnHealthsSet -= SetHealthHoldersRequest;
            Registry.Clear();
        }

        // public void SetHealth(int towerID, int newHealth)
        // {
        //     Registry[towerID].Health = newHealth;
        //     Eventbus.HealthEvents.OnHealthChange?.Invoke(Registry[towerID].Health, towerID);
        // }
    }
}