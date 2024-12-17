using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerRelated
{
    public class MultiShooter : MonoBehaviour, ITowerRelatedElement
    {
        public int Id { get; set; }
        public ShootingTable shootingTable;
        public Transform[] faces;
        public void Initialize(int id)
        {
            Id = id;
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
