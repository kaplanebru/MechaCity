using DG.Tweening;
using UnityEngine;

namespace TowerRelated
{
    public class Floor : MonoBehaviour, ITowerRelatedElement
    {
        public Transform[] parts;
        public Transform gear;
        public FloorData Data;
        
        private float startPosY;
        private Quaternion startRot;
        private bool isMoving = false;
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
            transform.DOKill();
            isMoving = true;
            gear.gameObject.SetActive(true);
            gear.DOLocalMoveY(Data.OpenPosY, Data.Duration).OnComplete(()=>isMoving= false);
            
            parts[1].DOLocalRotateQuaternion(Quaternion.Euler(0, 180, 0), Data.Duration);
        }

        public void HideGear()
        {
            transform.DOKill();
            gear.DOLocalMoveY(startPosY, Data.Duration).OnComplete(() =>
            {
                if(!isMoving) 
                    gear.gameObject.SetActive(false); //todo: temp
            });
            parts[1].localRotation = startRot;
        }

        public void TurnOffGear()
        {
            gear.gameObject.SetActive(false);
        }
    }
}

