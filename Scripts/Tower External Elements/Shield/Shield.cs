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
            DisableAllParts();
            
            currentHeight = height;
            shieldObject.gameObject.SetActive(true);
            ShowShieldParts();
        }

        void ShowShieldParts()
        {
            ResetShieldParts();
            
            for (int i = 0; i < currentHeight; i++)
            {
                var part = shieldParts[i];
                part.gameObject.SetActive(true);
                part.DOLocalMoveY(CommonData.TowerHeightPerStep * (i), riseDuration);
            }
        }

        void ResetShieldParts()
        {
            for (var i = 0; i < currentHeight; i++)
            {
                shieldParts[i].transform.localPosition = Vector3.zero;
            }
        }

       

        void DisableAllParts()
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
