using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace TowerRelated
{
    public class MultiShooter : MonoBehaviour, ITowerRelatedElement
    {
        public int Id { get; set; }
        public ShootingTable shootingTable;
        public Transform[] faces;
        public BPTimingData timingData;
        public void Initialize(int id)
        {
            Id = id;
            shootingTable.Setup(timingData.DurationByType[BpType.MultiShot]);
        }

        public void ShowShootingTable()
        {
            shootingTable.Reveal();
        }

        public void RevealNewShooter(int index)
        {
            faces[index].gameObject.SetActive(true);
        }
    }

}
