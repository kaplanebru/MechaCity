using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //TODO: Use ObjectPooling later
    //tower sayısı kadar projectile olur, ana towerda doğar, targeta ulaşınca deactive edilip poola geri döner
    private void OnEnable()
    {
        //Eventbus.FireEvents.OnShooting += ShootProjectile;
    }

    public void ShootProjectile(Vector3 targetPos, Action callback)
    {
        //TODO: projetile yuvası olmalı ya boydan hesaplanabilir (height - 0.5f)
        transform.DOMove(targetPos, 3).OnComplete(()=>
        {
            Destroy(gameObject);
            callback?.Invoke();
            //TODO REMOVE VİCTİM HEALTH HERE USING EVENT
        });
    }

    private void OnDisable()
    {
        //Eventbus.FireEvents.OnShooting -= ShootProjectile;
    }
}
