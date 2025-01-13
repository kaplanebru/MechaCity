using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using UnityEngine;

namespace TowerRelated
{
    public class Shield : MonoBehaviour, ITowerRelatedElement
    {
        public int Id { get; set; }
        public Transform shieldObject;
        public Fence[] fences;
        public List<Fence> openFences = new();
        public float riseDuration = 1;
        public CommonData CommonData;
        public BPTimingData timingData;

        private int currentHeight;
        public void Initialize(int id)
        {
            Id = id;
            foreach (var fence in fences)
            {
                fence.Setup(timingData.DurationByType[BpType.BreakShield]);
            }
        }

        public void RevealShield(int height)
        {
            DisableAllFences();
            
            currentHeight = height;
            shieldObject.gameObject.SetActive(true);
            ShowFences();
        }

        void ShowFences()
        {
            ResetFences();
            
            for (int i = 0; i < currentHeight; i++)
            {
                var fence = fences[i];
                openFences.Add(fence);
                fence.gameObject.SetActive(true);
                fence.transform.DOLocalMoveY(CommonData.TowerHeightPerStep * (i), timingData.DurationByType[BpType.Shield]);
            }
        }

        void ResetFences()
        {
            openFences.Clear();
            for (var i = 0; i < currentHeight; i++)
            {
                fences[i].transform.localPosition = Vector3.zero;
            }
        }
        
        void DisableAllFences()
        {
            foreach (var fence in fences)
            {
                fence.gameObject.SetActive(false);
            }
        }

        public void BreakShield()
        {
            foreach (var fence in openFences)
            {
                fence.Explode();
            }
        }

        
    }
}
