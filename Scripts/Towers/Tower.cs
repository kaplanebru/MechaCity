using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;
using DG.Tweening;

public class Tower : MonoBehaviour
{
     
    public TowerData Data;
    private MeshRenderer mesh;

    private void Start()
    {
        Setup();
    }

    void Setup()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
        StartRise();
        SetColor(Data.TeamData.DefaultMaterial); 
    }

    void StartRise()
    {
        transform.DOScaleY(Data.Height, 1);
    }

    public void SetColor(Material mat)
    {
        mesh.material = mat;
    }

    public void Attack(Tower other)
    {
        
    }

    private void OnMouseDown()
    {
        Click();
    }

    private void Click()
    {
        Eventbus.TowerEvents.OnTowerClicked?.Invoke(this);
    }
}
