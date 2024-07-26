using DG.Tweening;
using UnityEngine;

namespace TowerExternal
{
    public class Floor : MonoBehaviour, ITowerRelated, ITowerExternal
    {
        public Transform[] parts;
        public Transform gear;
        public FloorData Data;

        private float startHeight;
        private void OnEnable()
        {
            startHeight = parts[0].localScale.y;
        }

        public void Open( bool closeAtTheEnd = false)
        {
            gear.gameObject.SetActive(true);
        
            foreach (var part in parts)
            {
                part.DOScaleY(Data.OpenSize, Data.Duration).OnComplete(() =>
                    {
                        if (closeAtTheEnd)
                        {
                            DOVirtual.DelayedCall(Data.CloseDelay, () => RestoreHeight()); //todo: belki game started yazısı gelir
                        }
                    });
            }
        }

        public void RestoreHeight()
        {
            gear.gameObject.SetActive(false);
        
            foreach (var part in parts)
            {
                part.DOScaleY(startHeight, Data.Duration);
            }
        }

        public int Id { get; set; }
        public void Initialize(int id)
        {
            Id = id;
        }
    }
}

