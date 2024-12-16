using DG.Tweening;
using UnityEngine;

namespace TowerExternal
{
    public class Floor : MonoBehaviour, ITowerRelatedElement
    {
        public Transform[] parts;
        public Transform gear;
        public FloorData Data;
        
        private float startPosY;
        private Quaternion startRot;
        private void OnEnable()
        {
            startPosY = gear.transform.localPosition.y;
            startRot = parts[1].transform.localRotation;
        }
        
        public int Id { get; set; }
        public void Initialize(int id)
        {
            Id = id;
        }

        public void ShowGear()
        {
            gear.gameObject.SetActive(true);
            gear.DOLocalMoveY(Data.OpenPosY, Data.Duration);
            
            parts[1].DOLocalRotateQuaternion(Quaternion.Euler(0, 180, 0), Data.Duration);
        }

        public void HideGear()
        {
            gear.DOLocalMoveY(startPosY, Data.Duration).OnComplete(() =>
            {
                gear.gameObject.SetActive(false);
            });
            parts[1].localRotation = startRot;
        }

        public void TurnOffGear()
        {
            gear.gameObject.SetActive(false);
        }
    }
}

