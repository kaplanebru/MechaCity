using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;
using DG.Tweening;

public class Tower : MonoBehaviour
{
     
    public TowerData Data;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        StartRise();
        GetComponentInChildren<MeshRenderer>().material.color = Data.TeamData.Color;
    }

    void StartRise()
    {
        transform.DOScaleY(Data.Height, 1);
    }

    private void OnMouseDown()
    {
        Select();
    }

    private void Select()
    {
        Eventbus.TowerEvents.OnTowerSelected?.Invoke();
    }
}
