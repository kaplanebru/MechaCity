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
        // private static void ChangeHealth(IHealthy healthy, int newHealth)
        // {
        //     healthy.Health = newHealth;
        //     //UIEventbus.OnHealthChange.Invoke(newHealth, healthy.HealthID);
        // }
        //
        // public static void RemoveHealth(IHealthy healthy, int damage, Action completeCall)
        // {
        //     if (AllDoubles.TryInspectByTowerAndGetDouble(healthy.HealthID, out DoubleTower doubleTower))
        //         healthy = doubleTower;
        //     
        //     ChangeHealth(healthy, healthy.Health-damage);
        //     doubleTower.Shake();
        //     
        //     if(IsDead(healthy, completeCall)) return;
        //     completeCall();
        //     
        //     
        //     // if (AllDoubles.TryInspectByTowerAndGetDouble(healthy.HealthID, out DoubleTower doubleTower))
        //     // {
        //     //     ChangeHealth(doubleTower, doubleTower.Health-damage);
        //     //     doubleTower.Shake();
        //     //     if(IsDoubleDead(doubleTower, completeCall)) return;
        //     // }
        //     // else
        //     // {
        //     //     ChangeHealth(victimData, victimData.Health-damage);
        //     //     victimData.Shake();
        //     //     if(IsVictimDead(victimData, completeCall)) return;
        //     // }
        //
        //    
        // }
        

        // private static bool IsDoubleDead(DoubleTower doubleTower, Action completeCall)
        // {
        //     if (doubleTower.Health <= 0)
        //     {
        //         foreach (var towerID in doubleTower.towers)
        //         {
        //              var tower = AllTowers.GetTower(towerID.Key); 
        //              tower.HandleDeath( () => Eventbus.CombatEvents.OnTowerKilled?.Invoke(towerID.Key), completeCall);
        //         }
        //         return true;
        //     }
        //     return false;
        // }
        
        // private static bool IsDead(IHealthy healthy, Action completeCall)
        // {
        //     if (healthy.Health <= 0)
        //     {
        //         DeathOperator.Instance.HandleDeath(healthy.HealthSubjects, 
        //             () => Eventbus.CombatEvents.OnTowerKilled?.Invoke(healthy.HealthID), 
        //             completeCall);
        //         
        //         return true;
        //     }
        //     return false;
        // }
        
        // public static void ResetHealth(int id)
        // {
        //     if (AllDoubles.TryInspectByTowerAndGetDouble(id, out DoubleTower doubleTower))
        //     {
        //         foreach (var key in doubleTower.towers.Keys)
        //         {
        //             var newHealth =  AllTowers.GetTower(key).ConstantData.StartHealth;
        //             doubleTower.towers[key].Health = newHealth;
        //         }
        //         ChangeHealth(doubleTower, doubleTower.TotalHealth);
        //        
        //     }
        //     else
        //     {
        //         var towerObj = AllTowers.GetTower(id);
        //         var newHealth = towerObj.ConstantData.StartHealth;
        //         ChangeHealth(towerObj.Data, newHealth);
        //     }
        // }
  
    }

}
