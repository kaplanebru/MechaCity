using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TowerRelated
{
    public class ShootingTable : MonoBehaviour
    {
        public Transform[] parts;
        public float startHeight;

        public void Awake()
        {
            startHeight = parts[0].transform.localScale.x;
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
                scale.x = 0;
                part.transform.localScale = scale;
            }
        }

        void GrowParts()
        {
            foreach (var part in parts)
            {
                part.transform.DOScaleX(1, 1); //todo fix later
            }
        }
    }
}
