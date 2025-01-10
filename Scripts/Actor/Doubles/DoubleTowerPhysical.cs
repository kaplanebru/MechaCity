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
        }

        
        public void CreateBridge()
        {
            //_towerIDs = _towerNumerics.OrderBy(tower => tower.Height).Select(tower => tower.UniqID).ToArray();   
            //Eventbus.TowerEvents.OnBridgeAttempt?.Invoke(_towerIDs);
            //todo: aslında id'ye göre dizilmeli, 3lü tower olursa küçük ve büyük arası boşluk olabilir. Ama heighte göre dizilmezse de uzun olan kısalıyor falan

            _towerIDs = _towerNumerics.Select(tower => tower.UniqID).ToArray();
            Eventbus.TowerEvents.OnBridgeAttempt?.Invoke(_towerIDs);
          
            
            //önce id'ye göre diz
            //t1-t2 -- t2-t3 olacak şekilde grupla
            //grupları heighte göre orderla
            //her grup için event at
            
        }
        
       
        
        
        public void Shake()
        {
            //TODO İMPLEMENT LATER
        }
    }

}
