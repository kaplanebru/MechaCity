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
    
    public TowerData Data;
    public int Id { get; set; }
    public int health;
    public float height;
    public List<Tower> LinkedTowers;
    private MeshRenderer mesh;
    private int bulletAmount;
    public bool CanShoot { get; private set; }
    public int BulletAmount
    {
        get => bulletAmount;
        set
        {
            bulletAmount = value;
            CanShoot = value > 0;
        }
    }
    
    private void OnEnable()
    {
        Eventbus.FireEvents.OnFireEnabled += RestoreBullets;
    }
    
    public void Setup(BasePlayer player)
    {
        height = Data.StartHeight;
        health = Data.StartHealth;
        var towerModel = Instantiate(Data.Model, transform);
        mesh = towerModel.GetComponentInChildren<MeshRenderer>();
        StartRise();
        SetColor(player.Data.TeamData.DefaultMaterial);
    }

    void StartRise()
    {
        transform.DOScaleY(height, 1);
    }

    public void SetColor(Material mat)
    {
        mesh.material = mat;
    }

    private void RestoreBullets()
    {
        BulletAmount = Data.MaxBullet;
    }

    public void Descend(int amount)
    {
        height -= amount;
        transform.DOScaleY(height, 1);
    }

    public void Ascend(int amount)
    {
        height += amount;
        transform.DOScaleY(height, 1);
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