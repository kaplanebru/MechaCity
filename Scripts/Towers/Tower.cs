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

    public void Fight()
    {
        foreach (var other in Data.Pairs)
        {
            if (Data.Height > other.Data.Height)
            {
                other.Descend(Data.AttackAmount);
                break;
            }
            
            if(Data.Height < other.Data.Height)
            {
                Descend(other.Data.AttackAmount);
            }
        }
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
                Eventbus.FireEvents.OnPairsOrdered?.Invoke(new Pairs(this, other));
                break;
            }
            
            if(Data.Height < other.Data.Height)
                Eventbus.FireEvents.OnPairsOrdered?.Invoke(new Pairs(other, this));
            
        }
    }

    public List<Pairs> Pairs()
    {
        foreach (var other in Data.Pairs)
        {
            if (Data.Height > other.Data.Height)
            {
                //currentTower, other
                return new List<Pairs> {new Pairs(this, other)};

            }
            
            if(Data.Height < other.Data.Height)
            {
                //other, current
                //other, current

                Descend(other.Data.AttackAmount);
            }
        }
        return null;
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
}

public class Pairs
{
    public Tower Perpetrator { get; private set; }
    public Tower Victim { get; private set; }

    public Pairs(Tower _perpetrator, Tower _victim)
    {
        Perpetrator = _perpetrator;
        Victim = _victim;
    }
}
