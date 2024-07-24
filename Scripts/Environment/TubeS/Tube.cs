using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public int id;

    private MeshRenderer[] meshes;
   
    private void Awake()
    {
        meshes = GetComponentsInChildren<MeshRenderer>();
    }

    public void SetColor(Color color)
    {
        foreach (var mesh in meshes)
        {
            mesh.material.color = color;
        }
    }
}
