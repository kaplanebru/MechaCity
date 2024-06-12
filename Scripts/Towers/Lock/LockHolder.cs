using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHolder : MonoBehaviour, ITowerRelated
{
    public int Id { get; set; }

    public Lock[] locks;
    public CommonData commonData;
    private Lock _currentLock;
    public void Initialize(int id)
    {
        Id = id;
        _currentLock = locks[0];
        LockTower();
    }

    void DisableAll()
    {
        foreach (var llock in locks)
        {
            llock.gameObject.SetActive(false);
        }
    }
    void LockTower()
    {
        DisableAll();
        SetPosition();
        _currentLock.gameObject.SetActive(true);
    }

    void SetPosition()
    {
        _currentLock.transform.localPosition += Vector3.up * _currentLock.limit * commonData.TowerHeightPerStep;
    }
    
    
}
