using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHolder : MonoBehaviour, ITowerRelated
{
    public int Id { get; set; }

    public Lock[] locks;
    public void Initialize(int id)
    {
        Id = id;
    }
}
