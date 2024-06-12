using System.Collections;
using System.Collections.Generic;
using DataModels;
using DG.Tweening;
using GameUI;
using ProjectileHandler;
using Towers;
using UnityEngine;

public class Shooter : MonoBehaviour, ITowerRelated
{
    public float motionDistance = 1;
     float _duration = .5f;
    
    private float hiddenPosY;
    public int Id { get; set; }
    public void Initialize(int id)
    {
        Id = id;
        hiddenPosY = transform.localPosition.y;
    }

    private CombatPair _pair;

    public void SetDuration(float duration)
    {
        _duration = duration;
    }

    public void Shoot(CombatPair pair)
    {
        _pair = pair;
        RevealSelf();
    }

    public void RevealSelf()
    {
        transform.DOLocalMoveY(transform.localPosition.y + motionDistance, _duration).OnComplete(() => 
        {
            SendProjectile(_pair.MainTowerData, _pair.OtherTowerData, 1);
        });
    }

    private void Hide()
    {
        transform.DOLocalMoveY(hiddenPosY, _duration);
    }
    
    void SendProjectile(TowerData perpetrator, TowerData victim, float duration)
    {
        var projectile = ProjectilePool.Instance.GetItem(p =>
            p.transform.position = perpetrator.Mover.Data.Top.transform.position);
        projectile.Setup(duration, victim.Mover.Data.Top.transform.position - Vector3.up * .5f); //-Vector3.up

        perpetrator.BulletAmount--;

        projectile.Move(() =>
        {
            perpetrator.ColorHandler.ToOriginalColor();
            RemoveHealth(victim);
        });
    }
    
    void RemoveHealth(TowerData victimData)
    {
        victimData.Health -= _pair.OtherTowerData.DamagePower;
        UIEventbus.OnHealthChange.Invoke(victimData.Health, _pair.OtherTowerData.UniqID);
            
        victimData.Mover.Shake();

        if(IsVictimDead(victimData,  AllTowers.GetTower(victimData.UniqID)))
            return;
            
        _pair.CompleteCombat();
    }

    bool IsVictimDead(TowerData victimData, Tower victim)
    {
        if (victimData.Health <= 0)
        {
            victim.HandleDeath(() =>
                    Eventbus.CombatEvents.OnTowerKilled?.Invoke(victimData.UniqID),
                _pair.CompleteCombat);
            return true;
        }
        return false;
    }

   
}
