using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;



[Serializable]
public class TowerPartsData
{
    public Transform Top;
    public Transform Middle;
    public Transform Down;
}

public class TowerParts : MonoBehaviour
{
    private MeshRenderer[] meshes;
    public TowerPartsData Data;

    public void Setup()
    {
        meshes = Data.Middle.GetComponentsInChildren<MeshRenderer>();
    }

    public void SetColor(Material mat)
    {
        foreach (var mesh in meshes)
        {
            mesh.material = mat;
        }
    }
    
    public void ChangeHeight(float newHeight)
    {
        Data.Middle.transform.DOScaleY(newHeight, 1).OnComplete(() =>
        {
            Eventbus.UIEvents.OnTowerHeightChange?.Invoke(newHeight, gameObject);
        });
        Data.Top.transform.DOLocalMoveY(newHeight, 1);
        //down rotate
    }
}
