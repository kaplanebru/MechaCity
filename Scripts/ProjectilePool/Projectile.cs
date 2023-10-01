using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace ProjectileHandler
{
    public class Projectile : MonoBehaviour
    {
        //TODO: Use ObjectPooling later
        //tower sayısı kadar projectile olur, ana towerda doğar, targeta ulaşınca deactive edilip poola geri döner


        private float duration;
        private Vector3 targetPos;

        public void Setup(float _duration, Vector3 _targetPos)
        {
            duration = _duration;
            targetPos = _targetPos;
        }

        public void Move(Action callback)
        {
            var projectileLookRotation =
                Quaternion.LookRotation(new Vector3(targetPos.x, 0, targetPos.z) - transform.position);
            transform.rotation = projectileLookRotation;

            transform.DOMove(targetPos, duration).OnComplete(() =>
            {
                //Destroy(gameObject);
                ProjectilePool.Instance.ReleaseItem(this);
                callback?.Invoke();
            });
        }
    }
}