using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TowerMoverTest : MonoBehaviour
{
    public Transform activeHolder;
    public Transform passiveHolder;
    
    public List<Transform> passiveParts = new();
    public List<Transform> activeParts = new();

    [SerializeField] private int targetHeight = 2;

    private void Start()
    {
        DisableAll();
        
        Rise();
    }

    void Rise()
    {
        int step = targetHeight - Mathf.RoundToInt(activeHolder.transform.localPosition.y);

        RiseRoutine(step);
    }

    void RiseRoutine(int step)
    {
        GetNextPart();
        activeHolder.DOLocalMoveY(targetHeight-step+1, 1).OnComplete(() =>
        {
            step--;
            if(step > 0)
                RiseRoutine(step);
        });
    }

    void GetNextPart()
    {
        var nextPart = passiveParts.Last();
        
        passiveParts.Remove(nextPart);
        activeParts.Add(nextPart);
        
        nextPart.SetParent(activeHolder);
        RestoreOrder();
        nextPart.gameObject.SetActive(true);
    }

    void RestoreOrder()
    {
        for (var i = 0; i < activeParts.Count; i++)
        {
            var part = activeParts[i];
            var pos = part.transform.localPosition;
            pos.y = 0 - i;
            part.transform.localPosition = pos;
        }
    }

    void LoseLastPart()
    {
        var lastPart = activeParts.Last();
        activeParts.Remove(lastPart);
        passiveParts.Add(lastPart);
        
        lastPart.SetParent(passiveHolder);
        lastPart.gameObject.SetActive(false);
    }

    void DisableAll()
    {
        foreach (var passivePart in passiveParts)
        {
            passivePart.gameObject.SetActive(false);
        }
    }
}
