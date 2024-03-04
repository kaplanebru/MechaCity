
using DG.Tweening;
using Enums;
using Network;
using UnityEngine;

namespace Blueprint
{
    public class BPInteraction : MonoBehaviour
    {
        public Transform imageTransform;
        public Transform gear;
        
        private float startHeight;
    
        public Vector3 rot = new Vector3(0, 90, 0);
        private Vector3 startRot;
    
        public float hoverDuration = 1;
        public float selectDuration = 1;
        public float selectY = 0.1f;

        private BpType _currentBpType;
        private RotationEffect _rotationEffect;
        
        public void Setup(BpType currentType)
        {
            _currentBpType = currentType;
            Initialize();
        }
    
        void Initialize()
        {
            startHeight = transform.localPosition.y;
            startRot = gear.localEulerAngles;
            
            _rotationEffect = new RotationEffect(gear, rot, hoverDuration, startRot);
        }
        private void OnMouseEnter()
        {
            HoverImage();
        }
    
        private void OnMouseDown()
        {
            Select();
        }
    
        private void OnMouseExit()
        {
            ResetImage();
        }
    
        void HoverImage()
        {
            _rotationEffect.ExecuteRotation();
        }
    
        void ResetImage()
        {
            _rotationEffect.ResetRotation();
            
            // imageTransform.DOKill();
            // imageTransform.localEulerAngles = startRot;
        }
        void Select()
        {
            //ResetImage();
            transform.DOLocalMoveY(selectY, selectDuration/2).OnComplete(() =>
            {
                transform.DOLocalMoveY(startHeight, selectDuration/2);
            });
            
            NetworkEventbus.BlueprintEvents.OnBpSelected?.Invoke(_currentBpType);
        }
    }

}
