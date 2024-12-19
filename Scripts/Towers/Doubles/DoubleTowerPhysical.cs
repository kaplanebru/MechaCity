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

        private List<TowerHeightCouple> _towerHeightCouples = new();
        private List<TowerNumericData> _towers = new();
        private int _amount;
        public DoubleTowerPhysical(params uint[] actorIDs)
        {
            foreach (var actorID in actorIDs)
            {
                _towers.AddRange(ActorDB.GetTowersData(actorID).ToList());
                _towerHeightCouples.AddRange(ActorDB.GetTowerHeightCouples(actorID));
            }
            
           
            _towers = _towers.OrderBy(t => t.Height).ToList();
            _amount = _towers.Count;
        }

        public DoubleTowerPhysical(TowerNumericData[] towers)
        {
            _towers = towers.OrderBy(t => t.Height).ToList();
            _amount = _towers.Count;
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
            int totalHeight = 0;
            foreach (var tower in _towers)
            {
                totalHeight += tower.Height;
            }

            int averageHeight = totalHeight / _amount;
            int rest = totalHeight % averageHeight;

            for (var i = _towers.Count - 1; i >= 0; i--)
            {
                var tower = _towers[i];
                int extra = 0;
                if (rest > 0)
                {
                    extra = 1;
                    rest--;
                }

                var newHeight = averageHeight + extra;
                if (newHeight == tower.Height) continue;

                int surplus = newHeight - tower.Height;

                if (surplus == 0) continue;
                _towerHeightCouples[tower.UniqID].UpdateHeight(surplus);
                AllTowers.GetTower(tower.UniqID).StartRiseFallRoutine(true); //Todo: düzelt
            }
        }
        
        public void CreateBridge()
        {
            //_towerIDs = _towers.Select(t => t.UniqID).ToArray();
            _towerIDs = _towers.OrderBy(tower => tower.Height).Select(tower => tower.UniqID).ToArray();   
            Eventbus.TowerEvents.OnBridgeAttempt?.Invoke(_towerIDs);
        }
        
        public void Shake()
        {
            //TODO İMPLEMENT LATER
        }
    }

}
