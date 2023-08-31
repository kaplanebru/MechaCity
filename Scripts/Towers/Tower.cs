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
    
    public void Setup(TeamTowerData teamTowerData)
    {
        Data.Height = ConstantData.StartHeight;
        Data.Health = ConstantData.StartHealth;
        
        var towerModel = Instantiate(ConstantData.TowerAssetHolder.Model, transform);
        mesh = towerModel.GetComponentInChildren<MeshRenderer>();

        var healthIndicator = Instantiate(ConstantData.TowerAssetHolder.HealthIndicator, transform.position,
            Quaternion.identity);
        
        
        SetTeam(teamTowerData);
        StartRise();
    }

    void StartRise()
    {
        ChangeHeight(Data.Height);
    }

    public void SetColor(Material mat)
    {
        mesh.material = mat;
    }

    public void SetTeam(TeamTowerData teamTowerData)
    {
        Data.TeamTowerData = teamTowerData;
        SetColor(teamTowerData.DefaultMaterial);
    }
    
    public void ChangeHeight(float newHeight)
    {
        transform.DOScaleY(newHeight, 1).OnComplete(() =>
        {
            Eventbus.UIEvents.OnTowerHeightChange?.Invoke(newHeight, this);
        });
    }
    

    public void Descend(int amount)
    {
        Data.Height -= amount;
        ChangeHeight(Data.Height);
        
    }

    public void Ascend(int amount)
    {
        Data.Height += amount;
        ChangeHeight(Data.Height);
    }

    private void OnMouseDown()
    {
        if(Data.Clickable)
            Click();
    }

    private void Click()
    {
        Eventbus.TowerEvents.OnTowerClicked?.Invoke(this);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var linkedTower in Data.LinkedTowers)
        {
            Gizmos.DrawLine(transform.position, linkedTower.transform.position);
        }
    }
    
    private void RestoreBullets() //Todo: name change: bullet hakkı
    {
        Data.BulletAmount = ConstantData.MaxBullet;
    }

    private void OnDisable()
    {
        Eventbus.FireEvents.OnFireEnabled -= RestoreBullets;
    }
}