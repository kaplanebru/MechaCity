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
    public Transform modelHolder;

    
    private MeshRenderer mesh;



    private void OnEnable()
    {
        Eventbus.FireEvents.OnFireEnabled += RestoreBullets;
    }
    
    public void Setup(TeamTowerData teamTowerData)
    {
        Data.Height = ConstantData.StartHeight;
        
        Data.Health = ConstantData.StartHealth;
        Eventbus.UIEvents.OnHealthChange.Invoke(Data.Health, this);
        
        var towerModel = Instantiate(ConstantData.TowerAssetHolder.Model, modelHolder);
        mesh = towerModel.GetComponentInChildren<MeshRenderer>();

        // var healthIndicator = Instantiate(ConstantData.TowerAssetHolder.HealthIndicator, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1.7f),
        //     Quaternion.Euler(90, 0, 0)); //TODO: temp, bu kısım ui handlerda da yapılabilir
        
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
        modelHolder.transform.DOScaleY(newHeight, 1).OnComplete(() =>
        {
            Eventbus.UIEvents.OnTowerHeightChange?.Invoke(newHeight, this);
        });
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