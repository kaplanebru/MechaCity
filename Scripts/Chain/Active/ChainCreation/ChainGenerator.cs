using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Chain
{
    public interface IMachinePart
    {
        public IMachinePartData GetMoverData();
    }
    public class ChainGenerator : MonoBehaviour, IMachinePart
    {
        [HideInInspector]public ChainData ChainData;
        public List<ChainLink> links = new();
        public LinksPool linksPool;
        public void GenerateChain(Action saveCogs, Cogwheel[] chainRelatedCogs)
        {
            if (chainRelatedCogs.Length < 2)
            {
                ResetLinks();
                return;
            }

            foreach (var cog in chainRelatedCogs)
            {
                cog.Data.IsMoving = ChainData.IsMoving;
            }
            saveCogs(); 

            ChainData.CogAmount = chainRelatedCogs.Length;

            if(PoolNull()) return;
            ResetLinks();

            var chainGeneratorData = new ChainGeneratorData(ChainData, linksPool);
            links = new ChainPointCreator(chainRelatedCogs, chainGeneratorData).ExecutePhase();
        }

        public void CreateChainData(string chainDataName)
        {
            ChainData = ScriptableObject.CreateInstance<ChainData>();

            AssetDatabase.CreateAsset(ChainData, PathHelper.WriteAssetPath(chainDataName, "ChainDatas")); //(chainData, "Assets/chainData.asset"); 
            Debug.Log(PathHelper.WriteAssetPath(chainDataName, "ChainDatas"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public void ResetLinks() //direkt pooldan yapılabilir
        {
            if (linksPool.pool.Count == 0)
                PoolNull();
            links.ForEach(l => linksPool.ReleaseItem(l));
            links.Clear();
        }
        public void DeletePoolClearLinks() //on delete links
        {
            links.Clear();
            linksPool.DeletePool();
        }

        public LinksPool CreatePool()
        {
            linksPool = Instantiate(ChainData.LinksPoolPrefab, transform);
            return linksPool;
        }
        
        bool PoolNull()
        {
            if (linksPool == null)
            {
                linksPool = GetComponentInChildren<LinksPool>();
                if (linksPool == null)
                {
                    linksPool = CreatePool(); //added later
                    Debug.LogError("links pool null");
                    return true;
                }
            }
        
            if (linksPool.pool.Count == 0)
                linksPool.ActivatePool();
            
        
            if (links.Count > 0 && links.Any(l => l == null))
            {
                links.Clear();
                Debug.LogError("links null");
            }
                
            return false;
        }


        public IMachinePartData MachinePartData { get; set; }

        public IMachinePartData GetMoverData()
        {
            return (IMachinePartData)ChainData;
        }
    }
}

