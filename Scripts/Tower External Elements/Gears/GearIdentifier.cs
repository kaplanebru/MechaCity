using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TowerExternal
{
    public class GearIdentifier : MonoBehaviour, ITowerRelated
    {
        private Rotater _rotater;

        private void OnEnable()
        {
            _rotater = new Rotater(transform);
        }

        public void Rotate(float angle)
        {
            _rotater.Rotate(angle);
        }

        public int Id { get; set; }
        public void Initialize(int id)
        {
            Id = id;
            MediatorEventbus.SetupEvents.OnTowerIDSetting?.Invoke(Id, gameObject);
        }
    }

}
