using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //TODO: Use ObjectPooling later
    //tower sayısı kadar projectile olur, ana towerda doğar, targeta ulaşınca deactive edilip poola geri döner


    private float speed;
    private Vector3 targetPos;

    public void Setup(float _speed, Vector3 _targetPos)
    {
        speed = _speed;
        targetPos = _targetPos;
    }
    
    public void Move(Action callback)
    {
        var projectileLookRotation = Quaternion.LookRotation(new Vector3(targetPos.x, 0, targetPos.z)-transform.position);
        transform.rotation = projectileLookRotation;
        
        transform.DOMove(targetPos, speed).OnComplete(()=>
        {
            //Destroy(gameObject);
            ProjectilePool.Instance.ReleaseItem(this);
            callback?.Invoke();
        });
    }
    
}
