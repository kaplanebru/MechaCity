using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerRelated
{
    public class DisarmSign : MonoBehaviour, ITowerRelatedElement
    {
        public int Id { get; set; }
        public Transform disarmSignObject;
        public Transform[] parts;
        public BPTimingData timingData;
        public void Initialize(int id)
        {
            Id = id;
        }

        public void RevealSign()
        {
            disarmSignObject.gameObject.SetActive(true);
        }

        public void HideSign()
        {
            disarmSignObject.gameObject.SetActive(false);
        }
    }

}
