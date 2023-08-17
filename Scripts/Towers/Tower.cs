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
        victim.Descend(Data.DamagePower);
    }

    int bulletCounter = 0;
    void CreateCombatPairsByHeight()
    {
        List<CombatPair> combatPairs = new();
        bulletCounter = 0;
        
        foreach (var other in Data.LinkedTowers)
        {
            if (bulletCounter < Data.Bullet && Data.Height > other.Data.Height)
            {
                combatPairs.Add(new CombatPair(this, other));
                bulletCounter++;
            }
            else if(Data.Height < other.Data.Height)
                combatPairs.Add(new CombatPair(other, this));
            else
                combatPairs.Add(new CombatPair(other, this, true));
        }

        if (combatPairs.Count == 0) return;
        Eventbus.FireEvents.OnPairsAltered?.Invoke(combatPairs);
    }

    void CombatPairByHeight(Tower other, List<CombatPair> combatPairs)
    {
        if (bulletCounter < Data.Bullet && Data.Height > other.Data.Height)
        {
            combatPairs.Add(new CombatPair(this, other));
            bulletCounter++;
        }
        
        else if(Data.Height < other.Data.Height)
            combatPairs.Add(new CombatPair(other, this));
        else
            combatPairs.Add(new CombatPair(other, this, true));
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


