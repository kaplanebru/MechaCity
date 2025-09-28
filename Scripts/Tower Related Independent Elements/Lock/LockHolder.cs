using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHolder : MonoBehaviour, ITowerRelatedElement
{
    public int Id { get; set; }

    public Lock[] locks;
    public CommonData commonData;
    private Lock _currentLock;
    public void Initialize(int id)
    {
        Id = id;
        _currentLock = locks[0];
        //DisableAll();
    }
    
    public void LockTower(int limit)
    {
        SetPosition(limit);
        _currentLock.gameObject.SetActive(true);
    }

    void SetPosition(int limit)
    {
        _currentLock.transform.localPosition += Vector3.up * limit * commonData.TowerHeightPerStep;
    }

    public void UnlockTower()
    {
        _currentLock.gameObject.SetActive(false);
    }
    
    void DisableAll()
    {
        foreach (var llock in locks)
        {
            llock.gameObject.SetActive(false);
        }
    }
    
}
