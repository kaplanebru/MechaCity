using UnityEngine;

namespace TowerExternal
{
    public class Cable : MonoBehaviour
    {
        public int id;
    
        private SkinnedMeshRenderer[] meshes;
   
        private void Awake()
        {
            meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        public void SetColor(Color color)
        {
            foreach (var mesh in meshes)
            {
                mesh.material.color = color;
            }
        }
    }

}
