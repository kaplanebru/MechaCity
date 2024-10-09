using System;
using System.Collections;
using System.Collections.Generic;
using GameUI;
using Towers;
using UnityEngine;

namespace Health
{
    public class HealthHandler
    {
        public static void ChangeHealth(TowerData tower, int newHealth)
        {
            tower.Health = newHealth;
            UIEventbus.OnHealthChange.Invoke(newHealth, tower.UniqID);
        }

        public static void ChangeDoubleHealth(DoubleTower doubleTower, int newHealth)
        {
            doubleTower.Health = newHealth;
            UIEventbus.OnDoubleHealthChange?.Invoke(newHealth, doubleTower.ID);
        }
        
        public static void RemoveHealth(TowerData victimData, int damage, Action completeCall) 
        {
            if (AllDoubles.TryInspectByTowerAndGetDouble(victimData.UniqID, out DoubleTower doubleTower))
            {
                ChangeDoubleHealth(doubleTower, doubleTower.Health-damage);
                //todo: shake double
                if(IsDoubleDead(doubleTower, completeCall)) return;
                
            }
            else
            {
                ChangeHealth(victimData, victimData.Health-damage);
                victimData.Mover.Shake();
                if(IsVictimDead(victimData, completeCall)) return;
            }

            completeCall();
            //_pair.CompleteCombat();
        }

        public static bool IsDoubleDead(DoubleTower doubleTower, Action completeCall)
        {
            if (doubleTower.Health <= 0)
            {
                foreach (var towerID in doubleTower.towers)
                {
                     var tower = AllTowers.GetTower(towerID.Key); 
                     tower.HandleDeath( () => Eventbus.CombatEvents.OnTowerKilled?.Invoke(towerID.Key), completeCall);
                }
                return true;
            }
            return false;
        }
        public static bool IsVictimDead(TowerData victimData, Action completeCall)
        {
            if (victimData.Health <= 0)
            {
                var victim = AllTowers.GetTower(victimData.UniqID);
                victim.HandleDeath(() => Eventbus.CombatEvents.OnTowerKilled?.Invoke(victimData.UniqID), completeCall);
                return true;
            }
            return false;
        }
  
    }

}
