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
        public List<Transform> openParts = new();
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
                openParts.Add(part);
                part.gameObject.SetActive(true);
                part.DOLocalMoveY(CommonData.TowerHeightPerStep * (i), riseDuration);
            }
        }

        void ResetShieldParts()
        {
            openParts.Clear();
            for (var i = 0; i < currentHeight; i++)
            {
                shieldParts[i].transform.localPosition = Vector3.zero;
            }
        }
        
        void DisableAllParts()
        {
            foreach (var shieldPart in shieldParts)
            {
                shieldPart.gameObject.SetActive(false);
            }
        }

        public void BreakShield() //parçalanıp yere düşsün, sonra yok olsun
        {
            Debug.Log(openParts.Count);
            foreach (var part in openParts)
            {
                var smallParts = part.GetComponentsInChildren<Transform>(); //todo test
                foreach (var smallPart in smallParts)
                {
                    var pos = transform.position + Random.onUnitSphere * 4;
                    pos.y = 0;
                    smallPart.transform.position = pos;
                    smallPart.transform.rotation = Quaternion.Euler(GetRandomAngle(), GetRandomAngle(), GetRandomAngle());
                }
            }
        }

        float GetRandomAngle()
        {
            return Random.Range(0, 360);
        }
    }
}
