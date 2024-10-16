using System.Collections.Generic;
using System.Linq;
using Enums;
using Towers;

namespace Actor
{
    public class ActorData
    {
        public uint ID;
        public ActorType Type;
        public int[] TowerIDs;
        public TowerData[] Towers;
        public int TowerAmount { get; set; }
        
        public int Health;
        public int InitialHealth;
        
        public List<uint> LinkedActors = new();
        
        public ActorData(uint id, ActorType type, params int[] towerIDs)
        {
            ID = id;
            Type = type;

            SetTowers(towerIDs);
          
        }

        void SetTowers(params int[] towerIDs)
        {
            TowerIDs = towerIDs;
            Towers = new TowerData[TowerIDs.Length]; //TODO: make dict int,Data
            TowerAmount = Towers.Length;
          

            for (var i = 0; i < TowerIDs.Length; i++)
            {
                TowerData tower = AllTowers.GetData(TowerIDs[i]);
                Towers[i] = tower;
            }
            Towers = Towers.OrderBy(t => t.AvailableHeight).ToArray(); //İD'NİN LİNKAGE İÇİN YER DEĞİŞTİRMEMESİ Gerekebilir
        }
        

        public void SetLinkedTowers(params uint[] linkedActors)
        {
            LinkedActors = linkedActors.ToList();
        }
        public int GetFreeResource(int step) =>  TowerAmount * step;
        public int TryGetAvailableHeight(int step)
        {
            int availableHeight = Towers.Sum(tower => tower.AvailableHeight);
            return Towers[0].AvailableHeight < step ? 0 : availableHeight;
        }
    }
}