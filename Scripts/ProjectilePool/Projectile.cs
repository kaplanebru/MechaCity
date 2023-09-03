using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //TODO: Use ObjectPooling later
    //tower sayısı kadar projectile olur, ana towerda doğar, targeta ulaşınca deactive edilip poola geri döner
    

    public void Move(Vector3 targetPos, Action callback)
    {
        //TODO: projetile yuvası olmalı ya boydan hesaplanabilir (height - 0.5f)
        
        var projectileLookRotation = Quaternion.LookRotation(targetPos-transform.position);
        transform.rotation = projectileLookRotation;
        
        transform.DOMove(targetPos, 3).OnComplete(()=>
        {
            //Destroy(gameObject);
            ProjectilePool.Instance.ReleaseItem(this);
            callback?.Invoke();
        });
    }
    
}
