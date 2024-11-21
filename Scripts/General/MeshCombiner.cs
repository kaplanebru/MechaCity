using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    
    public void CombineMeshes(Material combinedMat)
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        
        // Array to hold combine instances
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Matrix4x4 parentTransform = transform.worldToLocalMatrix;

        // Iterate through MeshFilters
        for (int i = 0; i < meshFilters.Length; i++)
        {
            // Skip the parent object
            if (meshFilters[i] == GetComponent<MeshFilter>()) 
                continue;

            Mesh mesh = meshFilters[i].sharedMesh;
            if (mesh == null) 
                continue;

            combine[i].mesh = mesh;
            combine[i].transform = parentTransform * meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false); // Disable child object
        }

        // Assign the combined mesh to the parent
        MeshFilter parentMeshFilter = GetComponent<MeshFilter>();
        if (parentMeshFilter == null)
            parentMeshFilter = gameObject.AddComponent<MeshFilter>();

        MeshRenderer parentRenderer = GetComponent<MeshRenderer>();
        if (parentRenderer == null)
            parentRenderer = gameObject.AddComponent<MeshRenderer>();

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine, true, true);
        parentMeshFilter.mesh = combinedMesh;

        parentRenderer.material = combinedMat;
        
    }
}
