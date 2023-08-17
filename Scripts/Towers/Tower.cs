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
        Eventbus.FireEvents.OnFireEnabled += CreateCombatPairsByHeight;
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
    
    public void SetCombatPairs()
    {
        Data.CombatPairs.Clear();
        int attackCounter = 0;
        
        foreach (var tower in Data.LinkedTowers)
        {
            if (attackCounter < Data.MaxAttackAmount && Data.Height > tower.Data.Height)
            {
                attackCounter++;
                Data.CombatPairs.Add(new CombatPair(this, tower));
            }
            
            else if(Data.Height < tower.Data.Height)
                Data.CombatPairs.Add(new CombatPair(tower, this));
        }
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
        victim.Descend(Data.AttackPower);
    }

    void CreateCombatPairsByHeight()
    {
        int attackCounter = 0;
        foreach (var other in Data.LinkedTowers)
        {
            if (attackCounter < Data.MaxAttackAmount && Data.Height > other.Data.Height)
            {
                Eventbus.FireEvents.OnPairsAltered?.Invoke(new CombatPair(this, other));
                attackCounter++;
            }
            else if(Data.Height < other.Data.Height)
                Eventbus.FireEvents.OnPairsAltered?.Invoke(new CombatPair(other, this));
            else
                Eventbus.FireEvents.OnPairsAltered?.Invoke(new CombatPair(this, other, true));
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
        Eventbus.FireEvents.OnFireEnabled -= CreateCombatPairsByHeight;
    }
}


