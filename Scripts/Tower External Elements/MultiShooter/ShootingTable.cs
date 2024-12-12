using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TowerExternal
{
    public class ShootingTable : MonoBehaviour
    {
        public Transform[] parts;
        public float startHeight;

        public void Awake()
        {
            startHeight = parts[0].transform.localScale.z;
        }

        public void Reveal()
        {
            ResetParts();
            gameObject.SetActive(true);
            GrowParts();
        }
        void ResetParts()
        {
            foreach (var part in parts)
            {
                var scale = part.transform.localScale;
                scale.z = 0;
                part.transform.localScale = scale;
            }
        }

        void GrowParts()
        {
            foreach (var part in parts)
            {
                part.transform.DOScaleZ(0.175401f, 1); //todo fix later
            }
        }
    }
}
