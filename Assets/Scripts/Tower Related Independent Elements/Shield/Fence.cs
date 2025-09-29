using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerRelated
{
    public class Fence : MonoBehaviour
    {
        public Transform[] parts;
        private float explodeTime;
   
        private void Awake()
        {
            parts = GetComponentsInChildren<Transform>();
        }

        public void Setup(float time)
        {
            explodeTime = time;
        }

        public void Explode()
        {
            foreach (var part in parts)
            {
                var pos = transform.position + Random.onUnitSphere * 4;
                pos.y = 0;
                
                var rot = RandomHelper.GetRandomRotation();
                
                part.transform.DOMove(pos, explodeTime).SetEase(Ease.OutExpo);
                part.transform.DORotateQuaternion(rot, explodeTime).OnComplete(()=>
                {
                    part.gameObject.SetActive(false);
                    //part.transform.DORotateQuaternion( Quaternion.Euler(90, rot.eulerAngles.y, rot.eulerAngles.z), 0.3f).OnComplete(() => part.gameObject.SetActive(false));
                });
            }
        }
    }
 
}
