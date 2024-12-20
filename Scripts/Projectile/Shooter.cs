using System.Collections;
using System.Collections.Generic;
using DataModels;
using DG.Tweening;
using Enums;
using GameUI;
using Health;
using ProjectileHandler;
using Towers;
using UnityEngine;

public class Shooter : MonoBehaviour, ITowerRelatedElement
{
    public float motionDistance = 1;
    public Transform shootingSlot;
    public Transform cover;
    
    private CombatPair _pair;
    private float _motionDuration;
    private float _projectileDuration;
    private Quaternion startRot;
    private Quaternion coverStartRot;
    
    private float hiddenPosY;
    public int Id { get; set; }
    public void Initialize(int id)
    {
        Id = id;
        hiddenPosY = transform.localPosition.y;
        startRot = transform.rotation;
        coverStartRot = cover.transform.localRotation;
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
        coverRoutine = cover.DOLocalRotateQuaternion(coverStartRot * Quaternion.Euler(90, 0, 0), .5f);
    }

    void CloseCover()
    {
        cover.DOLocalRotateQuaternion(coverStartRot, .5f);
    }
    public void RevealSelf()
    {
        var mainTower = AllTowers.GetTower(_pair.MainTowerData.UniqID);
        var otherTower = AllTowers.GetTower(_pair.OtherTowerData.UniqID);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(coverRoutine);
        sequence.Append(transform.DOLocalMoveY(transform.localPosition.y + motionDistance, _motionDuration));
        sequence.Append(transform.DORotateQuaternion(
            Quaternion.LookRotation(
               otherTower.Data.Mover.Data.Top.transform.position - transform.position
            ) * Quaternion.Euler(0, 180, 0), _motionDuration / 2
        ));

        sequence.AppendCallback(() =>
        {
            SendProjectile(mainTower, otherTower, _projectileDuration);
        });
    }

    private void Hide()
    {
        transform.DORotateQuaternion(startRot, _motionDuration/2).OnComplete(() =>
        {
            transform.DOLocalMoveY(hiddenPosY, _motionDuration);
            CloseCover();
        });
       
        
    }
    
    void SendProjectile(TowerObject perpetrator, TowerObject victim, float duration)
    {
        var projectile = ProjectilePool.Instance.GetItem(p => p.transform.position = shootingSlot.position);
        projectile.Setup(duration, victim.Data.Mover.Data.Top.transform.position - Vector3.up * 1.5f);

        projectile.Move(() =>
        {
            perpetrator.Data.ColorHandler.ToOriginalSelectionColor();

            Hide();
            ShieldData shieldData = victim.Data.VisualSupportedDatas[VisualDataType.Shield] as ShieldData;
            if (shieldData.HasEffectiveShield(victim.NumericData.Height))
            {
                //TODO: shield effect
                _pair.CompleteCombat();
                return;
            }
            Eventbus.HealthEvents.OnShoot?.Invoke(_pair.OtherActor.ID, perpetrator.NumericData.DamagePower, _pair.ID);
            
            //Hide();
        });
    }
}
