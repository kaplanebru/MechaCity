using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TowerExternal
{
    public class Shield : MonoBehaviour, ITowerRelated, ITowerExternal
    {
        public int Id { get; set; }
        public Transform shieldObject;
        public Transform[] shieldParts;
        public float riseDuration = 1;
        public CommonData CommonData;

        private int currentHeight;
        public void Initialize(int id)
        {
            Id = id;
        }

        public void RevealShield(int height)
        {
            currentHeight = height;
            shieldObject.gameObject.SetActive(true);
        }

        void ShowShieldParts()
        {
            for (int i = 0; i < currentHeight; i++)
            {
                var part = shieldParts[i];
                part.DOLocalMoveY(CommonData.TowerHeightPerStep * (i + 1), riseDuration);
            }
        }

        void ResetShieldParts() 
        {
            foreach (var shieldPart in shieldParts)
            {
                shieldPart.transform.localPosition = Vector3.zero;
            }
        }

        public void KillShield() //parçalanıp yere düşsün, sonra yok olsun
        {
            
        }
    }
}
