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
            var direction = (targetPos - transform.position).normalized;
            var projectileLookRotation = Quaternion.LookRotation(direction);
            transform.rotation = projectileLookRotation;

            transform.DOMove(targetPos-direction*0.6f, duration).SetEase(Ease.InSine).OnComplete(() =>
            {
                ProjectilePool.Instance.ReleaseItem(this);
                callback?.Invoke();
            });
        }
    }
}