using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    private MeshFilter parentMeshFilter;
    private MeshRenderer parentRenderer;

    public void SetMaterial(Material combinedMat)
    {
        parentRenderer.material = combinedMat;
    }
    public void CombineMeshes(out MeshRenderer newRenderer)
    {
        
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        // Array to hold combine instances
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Matrix4x4 parentTransform = transform.worldToLocalMatrix;

        // Iterate through MeshFilters
        parentMeshFilter = GetComponent<MeshFilter>();
        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i] == parentMeshFilter)
                continue;

            Mesh mesh = meshFilters[i].sharedMesh;
            if (mesh == null)
                continue;

            combine[i].mesh = mesh;
            combine[i].transform = parentTransform * meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false); // Disable child object
        }

        AssignCombinedMesh(combine, out newRenderer);

    }
    private void AssignCombinedMesh(CombineInstance[] combine,  out MeshRenderer newRenderer)
    {
        if (parentMeshFilter == null)
            parentMeshFilter = gameObject.AddComponent<MeshFilter>();

        parentRenderer = GetComponent<MeshRenderer>();
        if (parentRenderer == null)
            parentRenderer = gameObject.AddComponent<MeshRenderer>();

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine, true, true);
        parentMeshFilter.mesh = combinedMesh;
        newRenderer = parentRenderer;
    }

    
}