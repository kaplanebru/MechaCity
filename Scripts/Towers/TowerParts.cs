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
    public MeshRenderer[] MiddleMeshes;
    public MeshRenderer TopMesh;
}
public class TowerParts : MonoBehaviour
{
    public TowerPartsData Data;

    public void SetColor(Material mat)
    {
        foreach (var mesh in Data.MiddleMeshes)
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

    public void SetClickableIds(int id)
    {
        var clickables = GetComponentsInChildren<Clickable>();
        foreach (var clickable in clickables)
        {
            clickable.Id = id;
        }
    }

   
}
