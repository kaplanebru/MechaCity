using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [ExecuteInEditMode]
    public class HoleHolder : MonoBehaviour
    {
        public HoleAssetHolder assetHolder;
        public List<Hole> holes = new();
        public int currentHoleId;
        private Vector3 lastScale;

        private void OnEnable()
        {
            if(!Application.isPlaying)
                RestoreHoles();
        }

        void RestoreHoles()
        {
            if(holes.Count == assetHolder.HoleTypes.Count) return;

            if(holes.Count > 0 && currentHoleId < holes.Count)
                 lastScale = holes[currentHoleId].transform.localScale;
            
            
            //print(assetHolder.HoleTypes.Count);
            DeleteAll();
            foreach (var hole in assetHolder.HoleTypes)
            {
                holes.Add(Instantiate(hole, transform));
            }

            if(holes.Count > 0 && currentHoleId < holes.Count)
                holes[currentHoleId].transform.localScale = lastScale;
            ShowHole(currentHoleId);
        }

        void DeleteAll()
        {
            for (int i = holes.Count - 1; i >= 0; i--)
            {
                var hole = holes[i];
                holes.Remove(hole);
                if(hole != null)
                    DestroyImmediate(hole.gameObject);
            }
        }

        void DisableAll()
        {
            holes.ForEach(h=>h.gameObject.SetActive(false));
        }

        public Hole ShowHole(int i)
        {
            DisableAll();
            currentHoleId = i;
            if (i >= holes.Count)
                return null;
            holes[i].gameObject.SetActive(true);
            return holes[i];
        }
    }
}