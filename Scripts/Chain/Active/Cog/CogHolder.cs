using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Chain
{
    public interface CogComponent
    {
        public int CogId { get; set; }
    }

    [ExecuteInEditMode]
    public class CogHolder : MonoBehaviour
    {
        [HideInInspector] public Cogwheel cogPrefab;
        public List<Cogwheel> cogs;
        public int newCogIndex = 0;

        public void GetCogs(IEnumerable<Cogwheel> _cogs)
        {
            cogs = _cogs.ToList();
        }

        public Cogwheel[] RestoreCogsInEditor() => cogs.ToArray();


        public Cogwheel[] GetChainRelatedCogs()
        {
            return cogs.Where(c => c.Data.ContactType == ChainEnums.CogContactType.ChainRelated).ToArray();
        }

        public bool showGizmos = true;

        public void DisableAllGizmos()
        {
            cogs.ForEach(c => c.drawGizmos = false);
        }

        public void DrawGizmosOnSelectedCog(int i)
        {
            if (!showGizmos) return;

            DisableAllGizmos();
            cogs[i].drawGizmos = true;
        }

        public void AddCog(bool isNew, CogData cogData = null, string cogDataName = null)
        {
            var newCog = Instantiate(cogPrefab, transform);
            newCog.name = "Cog " + newCogIndex++;
            
            newCog.AddData(isNew ? CreateCogData(cogDataName) : cogData);
            newCog.transform.localPosition = NewAddedCogPos(newCog.Data.Radius);
            cogs.Add(newCog);
            newCog.Setup();
        }

        public CogData CreateCogData(string cogName)
        {
            var cogData = ScriptableObject.CreateInstance<CogData>();
            AssetDatabase.CreateAsset(cogData, PathHelper.WriteAssetPath(cogName, "CogDatas")); //cogName + " Data", "CogDatas")

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return cogData;
        }

        public void RemoveCog(int cogToDestroyIndex)
        {
            if (cogs.Count == 0 || cogs == null) return;
            var cogsToDestroy = cogs[cogToDestroyIndex];

            //ChainEvents.OnDeleteObject?.Invoke(cogsToDestroy.transform);
            DestroyImmediate(cogsToDestroy.gameObject);

            cogsToDestroy.gameObject.SetActive(false);
            cogs.Remove(cogsToDestroy);
        }

        Vector3 NewAddedCogPos(float radius)
        {
            Vector3 newPos;

            if (cogs.Count == 0) return Vector3.zero;

            var cogPositions = new Vector3[cogs.Count];
            for (int i = 0; i < cogs.Count; i++)
            {
                cogPositions[i] = cogs[i].transform.localPosition;
            }

            Vector3 center = TrigonometryHelper.Center(cogPositions);
            var outermostCog = cogs.OrderByDescending(c => Vector3.Distance(center, c.transform.localPosition)).First();

            float distanceFromCenter =
                Vector3.Distance(center, outermostCog.transform.localPosition); // + outermostCog.Data.Radius;


            CreatePos:
            newPos = TrigonometryHelper.CirclePoint(UnityEngine.Random.Range(0, 360), distanceFromCenter) + center;
            float offset = radius * 2 + 2;
            newPos += new Vector3(offset, 0, offset); //not: hiç offset olmazsa sonsuz döngüye girebiliyor

            foreach (var cog in cogs)
            {
                if (Vector3.Distance(newPos, cog.transform.localPosition) <= cog.Data.Radius + radius)
                    goto CreatePos;
            }

            return newPos;
        }


        private void OnDisable()
        {
            //ChainEvents.OnCogSetupRequest -= CogsReady;
        }
    }
}