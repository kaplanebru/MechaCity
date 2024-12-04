using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerExternal
{
    public class MultiShooter : MonoBehaviour, ITowerRelated, ITowerExternal
    {
        public int Id { get; set; }
        public Transform shootingTable;
        public Transform[] faces;
        public void Initialize(int id)
        {
            Id = id;
        }

        public void ShowShootingTable()
        {
            shootingTable.gameObject.SetActive(true);
        }

        public void RevealNewShooter(int index)
        {
            faces[index].gameObject.SetActive(true);
        }
    }

}
