using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chain
{
    public class ChainPointsTester : MonoBehaviour
    {
        private ChainData Data;
        
        public Transform testCube2Pb;
        public Transform testCubePb;
        public Transform testSpherePb;
        
        private List<Transform> testCubes = new();
        private Transform testSphere;
        private Transform testCube;

        private void OnEnable()
        {
            enabled = Data.OnTesting;
        }

        void DestroyTestObjects()
        {
            for (int i = testCubes.Count - 1; i >= 0; i--)
            {
                if (testCubes[i] != null)
                    DestroyImmediate(testCubes[i].gameObject);
            }
        }

        void LinearPointsTesting(Arc arc)
        {
            testCubes.Add(Instantiate(testCube2Pb, arc.arcPoints.Last(), Quaternion.identity));
        }
        
        void DebugTangentPoints( Vector3 tangent0, Vector3 tangent1)
        {
            // if (testSphere != null)
            // {
            //     DestroyImmediate(testSphere.gameObject);
            //     DestroyImmediate(testCube.gameObject);
            // }
            
            testCube = Instantiate(testCubePb, tangent0, Quaternion.identity);
            testSphere = Instantiate(testSpherePb, tangent1, Quaternion.identity);
            testSphere.transform.localScale *= 2;
        }
    }
}