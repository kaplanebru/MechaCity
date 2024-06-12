using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    public void SetDuration(float duration)
    {
        _duration = duration;
    }

    public void RevealSelf()
    {
        transform.DOLocalMoveY(transform.localPosition.y + motionDistance, _duration).OnComplete(Hide);
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
            //RemoveHealth(victim);
        });
    }

   
}
