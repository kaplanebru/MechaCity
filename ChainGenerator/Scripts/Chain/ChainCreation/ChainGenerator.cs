using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using ChainEditorHelper;
#endif
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
        //public ChainData ChainData;
        [HideInInspector]public ChainData ChainData;
        public List<ChainLink> links = new();
        public LinksPool linksPool;
        public int cogAmount;
        
        public IMachinePartData MachinePartData { get; set; }

        public IMachinePartData GetMoverData()
        {
            return (IMachinePartData)ChainData;
        }
        public void GenerateChain(Action saveCogs, Cogwheel[] chainRelatedCogs)
        {
            if (ChainData == null)
            {
                Debug.LogWarning("Please Add Chain Data");
                return;
            }
            if(ChainData.LinkInterval <= 0) return;
                cogAmount = chainRelatedCogs.Length;
            if(ChainData == null) return;
            if (ChainData.LinksPoolPrefab == null)
            {
                Debug.LogWarning("Add Link Pool");
                return;
            }
            
            if (chainRelatedCogs.Length < 2)
            {
                if(!PoolNull())
                    ResetLinks();
                return;
            }

            foreach (var cog in chainRelatedCogs)
            {
                cog.Data.IsMoving = ChainData.IsMoving;
            }
            if(saveCogs != null)
                saveCogs(); 


            if(PoolNull()) return;
            ResetLinks();

            var chainGeneratorData = new ChainGeneratorData(ChainData, linksPool);
            links = new ChainPointCreator(chainRelatedCogs, chainGeneratorData).ExecutePhase();
        }

        
        #if UNITY_EDITOR
        public void CreateChainData(string chainDataName)
        {
            ChainData = ScriptableObject.CreateInstance<ChainData>();

            AssetDatabase.CreateAsset(ChainData, PathHelper.WriteAssetPath(chainDataName, "ChainData"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        #endif
        
        public void ResetLinks() 
        {
            if (linksPool.pool.Count == 0)
                PoolNull();
            links.ForEach(l => linksPool.ReleaseItem(l));
            links.Clear();
        }
        public void DeletePoolClearLinks() 
        {
            links.Clear();
            if(linksPool != null)
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
                    linksPool = CreatePool(); 
                    print("Link pool created");
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
    }
}

