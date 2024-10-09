using System.Collections;
using System.Collections.Generic;
using GameUI;
using Towers;
using UnityEngine;

namespace Health
{
    public static class HealthHandler
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
        
        public static void RemoveHealth(TowerData victimData, int damage) 
        {
            if (AllDoubles.TryInspectByTowerAndGetDouble(victimData.UniqID, out DoubleTower doubleTower))
            {
                ChangeDoubleHealth(doubleTower, doubleTower.Health-damage);
                //todo: shake double
            }
            else
            {
                ChangeHealth(victimData, victimData.Health-damage);
                victimData.Mover.Shake();
            }
           
            //double ise hepsinin healthinin toplamından çıkarıcaz
            
            //TODO: double'ın tamamı sallanmalı

            if(IsVictimDead(victimData,  AllTowers.GetTower(victimData.UniqID)))
                return;
            
           // _pair.CompleteCombat();
        }
        
        public static bool IsVictimDead(TowerData victimData, Tower victim)
        {
            if (victimData.Health <= 0)
            {
                if (AllDoubles.TryInspectByTowerAndGetDouble(victimData.UniqID, out DoubleTower doubleTower))
                {
                    foreach (var towerID in doubleTower.towers)
                    {
                        // var tower = AllTowers.GetTower(towerID.Key); 
                        // tower.HandleDeath( () => Eventbus.CombatEvents.OnTowerKilled?.Invoke(towerID.Key), _pair.CompleteCombat);
                    }
                }
                else
                {
                    // victim.HandleDeath(() =>
                    //         Eventbus.CombatEvents.OnTowerKilled?.Invoke(victimData.UniqID),
                    //     _pair.CompleteCombat);
                }
            
                return true;
            }
            return false;
        }
  
    }

}
