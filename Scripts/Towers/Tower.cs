using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;
using DG.Tweening;
using Models;

public class Tower : MonoBehaviour
{
    //Shooter, RiserFall, Selectable Components diye 3'e ayrılabilir
    
    public TowerConstantData ConstantData;
    public TowerData Data;
    private MeshRenderer mesh;
  
    
    private void OnEnable()
    {
        Eventbus.FireEvents.OnFireEnabled += RestoreBullets;
    }
    
    public void Setup(BasePlayer player)
    {
        Data.Height = ConstantData.StartHeight;
        Data.Health = ConstantData.StartHealth;
        var towerModel = Instantiate(ConstantData.Model, transform);
        mesh = towerModel.GetComponentInChildren<MeshRenderer>();
        StartRise();
        SetColor(player.Data.TeamData.DefaultMaterial);
    }

    void StartRise()
    {
        transform.DOScaleY(Data.Height, 1);
    }

    public void SetColor(Material mat)
    {
        mesh.material = mat;
    }

    private void RestoreBullets()
    {
        Data.BulletAmount = ConstantData.MaxBullet;
    }

    public void Descend(int amount)
    {
        Data.Height -= amount;
        transform.DOScaleY(Data.Height, 1);
    }

    public void Ascend(int amount)
    {
        Data.Height += amount;
        transform.DOScaleY(Data.Height, 1);
    }

    private void OnMouseDown()
    {
        Click();
    }

    private void Click()
    {
        Eventbus.TowerEvents.OnTowerClicked?.Invoke(this);
    }

    private void OnDisable()
    {
        Eventbus.FireEvents.OnFireEnabled -= RestoreBullets;
    }
}