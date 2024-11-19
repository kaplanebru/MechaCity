using System.Collections;
using System.Collections.Generic;
using DataModels;
using DG.Tweening;
using GameUI;
using Health;
using ProjectileHandler;
using Towers;
using UnityEngine;

public class Shooter : MonoBehaviour, ITowerRelated
{
    public float motionDistance = 1;
    public Transform shootingSlot;
    public Transform cover;
    
    private CombatPair _pair;
    private float _motionDuration;
    private float _projectileDuration;
    
    private float hiddenPosY;
    public int Id { get; set; }
    public void Initialize(int id)
    {
        Id = id;
        hiddenPosY = transform.localPosition.y;
    }


    public void SetDuration(float motionDuration, float projectileDuration)
    {
        _motionDuration = motionDuration;
        _projectileDuration = projectileDuration;
    }

    public void Shoot(CombatPair pair)
    {
        _pair = pair;
        OpenCover();
        RevealSelf();
    }

    private Tweener coverRoutine;
    void OpenCover()
    {
        coverRoutine = cover.DOLocalRotate(new Vector3(90, 0, 0), .5f);
    }

    void CloseCover()
    {
        cover.DOLocalRotate(Vector3.zero, .5f);
    }
    public void RevealSelf()
    {
        coverRoutine.OnComplete(() =>
        {
            transform.DOLocalMoveY(transform.localPosition.y + motionDistance, _motionDuration).OnComplete(() =>
            {
                SendProjectile(_pair.MainTowerData, _pair.OtherTowerData, _projectileDuration);
            });
        });
    }

    private void Hide()
    {
        transform.DOLocalMoveY(hiddenPosY, _motionDuration);
        CloseCover();
    }
    
    void SendProjectile(TowerData perpetrator, TowerData victim, float duration)
    {
        var projectile = ProjectilePool.Instance.GetItem(p => p.transform.position = shootingSlot.position);
        projectile.Setup(duration, victim.Mover.Data.Top.transform.position - Vector3.up * 1.5f); 

        perpetrator.BulletAmount--;

        projectile.Move(() =>
        {
            perpetrator.ColorHandler.ToOriginalColor();
           
            Eventbus.HealthEvents.OnShoot?.Invoke(_pair.OtherActor.ID, perpetrator.DamagePower, _pair.CompleteCombat);
            
            Hide();
        });
    }
}
