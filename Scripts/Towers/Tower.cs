using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;
using DG.Tweening;
using Models;

public class Tower : MonoBehaviour
{
    public TowerData Data;
    private MeshRenderer mesh;

    private void OnEnable()
    {
        Eventbus.FireEvents.OnFireEnabled += OrderPairsByHeight;
    }

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
    
    public void Attack(Tower victim)
    {
        //shooting anim
        victim.Descend(Data.AttackAmount);
    }

    void OrderPairsByHeight()
    {
        foreach (var other in Data.Pairs)
        {
            if (Data.Height > other.Data.Height)
            {
                Eventbus.FireEvents.OnPairsOrdered?.Invoke(new CombatPair(this, other));
                break;
            }
            
            if(Data.Height < other.Data.Height)
                Eventbus.FireEvents.OnPairsOrdered?.Invoke(new CombatPair(other, this));
        }
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
        Eventbus.FireEvents.OnFireEnabled -= OrderPairsByHeight;
    }
}


