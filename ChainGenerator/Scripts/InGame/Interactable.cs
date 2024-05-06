using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEngine;

namespace ChainInGame
{
    public class Interactable : MonoBehaviour
    {
        public Cogwheel _gear;
        int _id;
        private BoxCollider _collider;
        public void Setup(Cogwheel gear, int id)
        {
            _id = id;
            _gear = gear;
            AddCollider();
            SetSize();
        }
    
        void AddCollider()
        {
            _collider = gameObject.AddComponent<BoxCollider>();
        }

        void SetSize()
        {
            float size = _gear.Data.Radius * 1.5f;
            _collider.size = new Vector3(size, 1, size);
        }
    }

}
