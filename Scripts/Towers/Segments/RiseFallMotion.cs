using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class RiseFallData
{
    public Transform ActiveHolder;
    public Transform PassiveHolder;

    public List<Transform> PassiveParts = new();
    public List<Transform> ActiveParts = new();

    public bool IsRising;
    public float TargetHeight;
}

public class RiseFallMotion
{
    private RiseFallData Data;

    public float speed = 0.025f;


    public RiseFallMotion(RiseFallData data)
    {
        Data = data;
    }

    public void UpdateData(float newHeight, bool isRising)
    {
        Data.TargetHeight = newHeight;
        Data.IsRising = isRising;
    }

    private float startHeight;
    public IEnumerator RiseRoutine()
    {
        DisableAll();
        
        while (true)
        {
            startHeight = Data.ActiveHolder.localPosition.y;
            if (Data.IsRising)
            {
                while (Data.ActiveHolder.localPosition.y < Data.TargetHeight)
                {
                    Vector3 pos = Data.ActiveHolder.localPosition;
                    Move(pos);
                    
                    if (Data.ActiveHolder.localPosition.y >= startHeight)
                    {
                        startHeight = Data.ActiveHolder.localPosition.y + 1;
                        if (Data.PassiveParts.Count == 0) break;

                        GetNextPart();
                    }
                    yield return null;
                }
            }

            else
            {
                while (Data.ActiveHolder.localPosition.y > Data.TargetHeight)
                {
                    Vector3 pos = Data.ActiveHolder.localPosition;
                    Move(pos);

                    Debug.Log("start: " + startHeight);
                    if (Data.ActiveHolder.localPosition.y <= startHeight - 1)
                    {
                        startHeight = Data.ActiveHolder.localPosition.y;
                        Debug.Log("start: " + startHeight + " target: " + Data.TargetHeight);
                        if (Data.ActiveParts.Count == 0) break;
                        
                        LoseLastPart();
                    }
                    
                    yield return null;
                }
                
            }

            yield return null;
        }
    }


    void Move(Vector3 pos)
    {
        pos.y = Mathf.MoveTowards(pos.y, Data.TargetHeight, speed);
        Data.ActiveHolder.localPosition = pos;
    }


    void GetNextPart()
    {
        var nextPart = Data.PassiveParts.Last();

        Data.PassiveParts.Remove(nextPart);
        Data.ActiveParts.Add(nextPart);

        nextPart.SetParent(Data.ActiveHolder);
        RestoreOrder();
    }

    void LoseLastPart()
    {
        var lastPart = Data.ActiveParts.Last();
        Data.ActiveParts.Remove(lastPart);
        Data.PassiveParts.Add(lastPart);

        lastPart.SetParent(Data.PassiveHolder);
        lastPart.gameObject.SetActive(false);
        lastPart.localPosition = Vector3.zero;
    }

    void RestoreOrder()
    {
        for (var i = 0; i < Data.ActiveParts.Count; i++)
        {
            var part = Data.ActiveParts[i];
            var pos = part.transform.localPosition;
            pos.y = 0 - i;
            part.transform.localPosition = pos;
        }

        Data.ActiveParts.Last().gameObject.SetActive(true);
    }


    void DisableAll()
    {
        foreach (var passivePart in Data.PassiveParts)
        {
            passivePart.gameObject.SetActive(false);
        }
    }

    #region Lerp

    /* LERP
 IEnumerator MoveRoutine()
    {
        int step = 0;
        while (true)
        {
            while ((Mathf.Abs(targetHeight - activeHolder.localPosition.y) > 0.001f)) //activeHolder.localPosition.y < targetHeight
            {
                Vector3 pos = activeHolder.localPosition;
                pos.y = Mathf.Lerp(pos.y, targetHeight, 0.025f);
                activeHolder.localPosition = pos;

                if (pos.y >= step)
                {
                    step++;
                    if (passiveParts.Count == 0) yield break;
                    GetNextPart();
                }

                yield return null;
            }

            activeHolder.localPosition =
                new Vector3(activeHolder.localPosition.x, targetHeight, activeHolder.localPosition.z);
            yield return null;
        }
    }*/

    #endregion
}