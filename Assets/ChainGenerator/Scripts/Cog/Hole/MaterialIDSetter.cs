using System.Linq;
using UnityEngine;

namespace Chain
{
    //[ExecuteInEditMode]
    public class MaterialIDSetter : MonoBehaviour
    {
        public Cogwheel gear;
        private Material _gearMat;
    
        private readonly int _materialID = Shader.PropertyToID("_MaterialID");
        public bool isBackwards = false;
    
        void OnEnable()
        {
            Setup();
            SetID();
            SetID();
        }
        
        void Setup()
        {
            gear = GetComponent<Cogwheel>();
            if (gear == null) return;

            _gearMat = gear.cogObject.GetComponentInChildren<MeshRenderer>().material;
        }

        void SetID()
        {
            var id = GetInstanceID();
            _gearMat.SetFloat(_materialID, id);
            
           
            
            if(gear.holeHolder == null) return;
            foreach (var hole in gear.holeHolder.holes)
            {
                var currentHoleMesh = hole.GetComponentsInChildren<MeshRenderer>()
                    .FirstOrDefault(m => m.material.shader.name == "Custom/Hole");

                if (currentHoleMesh == null)
                {
                    print("null");
                    continue;
                }
                currentHoleMesh.material.SetFloat(_materialID, id);
            
                
               
                currentHoleMesh.material.renderQueue = 2000 + gear.sortingOrder;
                //print(currentHoleMesh.material.renderQueue);
            }
        }
    }
}
