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
        
    }

    private int click = 0;

    IEnumerator RiseCheckRoutine()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                click++;
                if (!isMoving)
                {
                    
                    Rise();
                }
                else
                {
                
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            click++;
            if (!isMoving)
            {
                targetHeight = click;
                Rise();
            }
            
        }
    }

    void Rise()
    {
        int step = targetHeight - Mathf.RoundToInt(activeHolder.transform.localPosition.y);
        if (step == 1)
        {
            RiseOneStep();
            return;
        }
        
        RiseMultipleSteps(step);
        
        
    }

    void RiseMultipleSteps(int step)
    {
        GetNextPart();
        float duration = 1 * step;
        activeHolder.DOLocalMoveY(targetHeight, duration).OnUpdate(() =>
        {
            if (activeHolder.transform.localPosition.y > targetHeight - step + 1)
            {
                step--;
                GetNextPart();
            }
        });
      
    }

    private bool isMoving = false;
    void RiseOneStep()
    {
        isMoving = true;
        GetNextPart();
        activeHolder.DOLocalMoveY(targetHeight, 1).OnComplete(() =>
        {
            isMoving = false;
            if (click > targetHeight)
            {
                targetHeight = click;
                Rise();
            }
                
        });
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
        
        // Sequence sequence = DOTween.Sequence();
        //
        // for (int i = 0; i < step; i++)
        // {
        //     sequence.AppendCallback(() => GetNextPart());
        //     sequence.Append(activeHolder.DOLocalMoveY( i+1, 1));
        // }
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
