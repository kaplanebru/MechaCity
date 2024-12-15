using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerExternal
{
    public class DisarmSign : MonoBehaviour, ITowerRelated, ITowerExternal
    {
        public int Id { get; set; }
        public Transform disarmSignObject;
        public Transform[] parts;
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
