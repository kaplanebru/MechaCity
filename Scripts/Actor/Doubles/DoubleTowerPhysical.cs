using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using UnityEngine;

namespace Towers
{
    public class DoubleTowerPhysical
    {
        private int[] _towerIDs;

        private List<TowerData> _towerDatas = new();
        private List<TowerNumericData> _towerNumerics = new();
        private int _amount;
        public DoubleTowerPhysical(params uint[] actorIDs)
        {
            Debug.Log("this is a new double");
            foreach (var actorID in actorIDs)
            {
                _towerNumerics.AddRange(ActorDB.GetTowersNumericData(actorID).ToList());
                _towerDatas.AddRange(ActorDB.GetTowersData(actorID));
            }
            
           
            _towerNumerics = _towerNumerics.OrderBy(t => t.Height).ToList();
            _amount = _towerNumerics.Count;
        }

        public DoubleTowerPhysical(TowerNumericData[] numerics, TowerData[] towerDatas)
        {
            _towerNumerics = numerics.OrderBy(t => t.Height).ToList();
            _towerDatas = towerDatas.OrderBy(t => t.NumericData.Height).ToList();
            _amount = _towerNumerics.Count;
        }

        private void SeRegarde() //iptal, arkasını dönsün istemeyiz
        {
            // for (var i = 0; i < _towers.Count; i++)
            // {
            //     var tower = _towers[i];
            //     tower.Mover.OrientVersTarget();
            // }
        }
        
        public void Equalize() //bridgeden önce olmalı
        {
            DoubleTowerEqualizer.Equalize(_towerDatas.ToArray());
            
            //  int totalHeight = 0;
            //  foreach (var tower in _towerNumerics)
            //  {
            //      totalHeight += tower.Height;
            //  }
            //
            //  int averageHeight = totalHeight / _amount;
            //  int rest = totalHeight % averageHeight;
            //
            //  for (var i = _towerNumerics.Count - 1; i >= 0; i--)
            //  {
            //      var tower = _towerNumerics[i];
            //      int extra = 0;
            //      if (rest > 0)
            //      {
            //          extra = 1;
            //          rest--;
            //      }
            //
            //      var newHeight = averageHeight + extra;
            //      if (newHeight == tower.Height) continue;
            //
            //      int surplus = newHeight - tower.Height;
            //
            //      if (surplus == 0) continue;
            //      _towerDatas[tower.UniqID].UpdateHeight(surplus);
            //      AllTowers.GetTower(tower.UniqID).StartRiseFallRoutine(true); //Todo: düzelt
            // }
        }
        
        public void CreateBridge()
        {
            _towerIDs = _towerNumerics.OrderBy(tower => tower.Height).Select(tower => tower.UniqID).ToArray();   
            Eventbus.TowerEvents.OnBridgeAttempt?.Invoke(_towerIDs);
        }
        
        public void Shake()
        {
            //TODO İMPLEMENT LATER
        }
    }

}
