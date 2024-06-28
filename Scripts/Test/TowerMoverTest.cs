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

    [SerializeField] private int targetHeight = 0;
    private bool isMoving = false;
    private int step = 0;

    private void Start()
    {
        DisableAll();
        StartCoroutine(MoveRoutine());
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            targetHeight++;
            isMoving = true;

            //step = targetHeight - Mathf.RoundToInt(activeHolder.transform.localPosition.y);
        }
    }

    public float duration = 1f;

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            
                while (activeHolder.localPosition.y < targetHeight)
                {
                    activeHolder.localPosition = new Vector3(activeHolder.localPosition.x,
                        Mathf.MoveTowards(activeHolder.localPosition.y, targetHeight, 0.025f),
                        activeHolder.localPosition.z);

                    if (activeHolder.localPosition.y >= step)
                    {
                        step++;
                        GetNextPart();
                    }

                    yield return null;
                }

                yield return null;
        }
    }

    


    void GetNextPart()
    {
        var nextPart = passiveParts.Last();

        passiveParts.Remove(nextPart);
        activeParts.Add(nextPart);

        nextPart.SetParent(activeHolder);
        RestoreOrder();
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

        activeParts.Last().gameObject.SetActive(true);
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

    #region Multiple

// void RiseOneStep()
// {
//     //isMoving = true;
//     GetNextPart();
//     activeHolder.DOLocalMoveY(targetHeight, 1).OnComplete(() =>
//     {
//         // isMoving = false;
//         // if (click > targetHeight)
//         // {
//         //     targetHeight = click;
//         //     Rise();
//         // }
//             
//     });
//     
//     //TODO: ASLINDA ORDERDA SORUN ÇIKIYOR. ORDER KISMINA ŞERH DÜŞMEK LAZIM İS MOVİNG FALAN DİYE
// }

    void RiseRoutine(int step)
    {
        GetNextPart();
        activeHolder.DOLocalMoveY(targetHeight - step + 1, 1).OnComplete(() =>
        {
            step--;
            if (step > 0)
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

    #endregion
}