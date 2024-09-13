using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Towers;
using Turn;
using UnityEngine;

namespace Blueprint
{
    public class DoubleSelfAction : IBpAction
    {
   
        public void Execute(params object[] obj)
        {
            
            var selectedTowers = (int[]) obj[0];
            
            CreateBridge(selectedTowers);
            
            BpEventbus.ActionEvents.OnDoubleSelfAction.Invoke(new DoubleTower(selectedTowers));
            //Eventbus.LinkEvents.OnDoubleSelfAction?.Invoke(selectedTowers);

        }
        public void Restore(params object[] obj)
        {
            //sonsuza kadar(ölene) double kalacaksa gerek yok
        }

        List<TowerData> couple = new();
        
        void CreateBridge(int[] towers)
        {
            TowerData[] towerGroup = AllTowers.GetTowerGroup(towers); //.OrderBy(t => t.Height).ToArray();
            
            for (int i = 0; i < towerGroup.Length-1; i++)
            {
                couple.Add(towerGroup[i]);
                couple.Add(towerGroup[i+1]);
                couple = couple.OrderBy(t => t.Height).ToList();
                
                Eventbus.TowerEvents.OnBridgeAttempt?.Invoke(couple[0].UniqID, couple[1].UniqID);
                //couple[i].Uzan
                
                couple.Clear();
            }
        }
    }

}
