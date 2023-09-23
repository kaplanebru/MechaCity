using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;
using DG.Tweening;
using Models;

//[RequireComponent(typeof(TowerParts))]
public class Tower : MonoBehaviour
{
    //Shooter, RiserFall, Selectable Components diye 3'e ayrılabilir
    
    public TowerConstantData ConstantData;
    public TowerData Data;
    public TowerParts towerParts;
   
    
    private void OnEnable()
    {
        Eventbus.FireEvents.OnFireEnabled += RestoreBullets;
        towerParts = GetComponent<TowerParts>();
    }
    
    public void Setup(TeamTowerData teamTowerData)
    {
        Data.Height = ConstantData.StartHeight;
        
        Data.Health = ConstantData.StartHealth;
        Eventbus.UIEvents.OnHealthChange.Invoke(Data.Health, this);
        
        SetTeam(teamTowerData);

        towerParts.ChangeHeight(Data.Height); //FirstRise

        Eventbus.TowerEvents.OnTowerSetup?.Invoke(this);
    }

    public void SetTeam(TeamTowerData teamTowerData)
    {
        Data.TeamTowerData = teamTowerData;
        towerParts.SetColor(teamTowerData.DefaultMaterial);
        Eventbus.TeamEvents.OnTowerTeamSet?.Invoke(teamTowerData.TeamType, this);
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