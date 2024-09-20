using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameUI;
using UnityEngine;

[Serializable]
public class RiseFallData
{
    public int Id { get; private set; }
    public Transform ActiveHolder;
    public Transform PassiveHolder;

    public List<Transform> PassiveParts = new();
    public List<Transform> ActiveParts = new();
    
    public RiseState RiseState;
    public float TargetHeight;

    public CommonData CommonData;

    public void SetId(int id)
    {
        Id = id;
    }
}

public enum RiseState
{
    Rising,
    Falling,
    None
}

public class RiseFallMotion
{
    private RiseFallData Data;

    public float speed = 0.025f;
    public float unit; // = 1.8f;
    float tolerance = 0.0001f;

    private float startHeight;


    public void SetId(int id)
    {
        Data.SetId(id);
    }
    public RiseFallMotion(RiseFallData data)
    {
        Data = data;
        unit = Data.CommonData.TowerHeightPerStep;
    }

    public void UpdateData(float newHeight, bool isRising)
    {
        Data.TargetHeight = newHeight;
        Data.RiseState = isRising ? RiseState.Rising : RiseState.Falling;
    }

    float RoundByCustomUnit(float number)
    {
        float residue = number % unit;
        float result = residue > 0 ? number - residue + unit : number;
        return result;
    }

    public void SetZeroHeight(int y)
    {
        var pos = Data.ActiveHolder.localPosition;
        pos.y = y;
        Data.ActiveHolder.localPosition = pos;
    }

    public IEnumerator RiseRoutine()
    {
        DisableAll();

       // Debug.Log("routine: " + Data.Id);
        while (true)
        {

            startHeight = RoundByCustomUnit(Data.ActiveHolder.localPosition.y);
            if (Data.RiseState == RiseState.Rising)
            {
                //Debug.Log("rising: " + Data.Id);
                while (Data.ActiveHolder.localPosition.y < Data.TargetHeight)
                {
                    if (Data.ActiveHolder.localPosition.y >= startHeight)
                    {
                        if (Data.PassiveParts.Count == 0)
                        {
                            Data.RiseState = RiseState.None;
                            break;
                        }

                        startHeight += unit;
                        GetNextPart();
                    }

                    Move(Data.ActiveHolder.localPosition);
                    //not: bu loop'un içindeyken state değişimini kaçırıyor.target height değiştiği için looptan çıkılıyor ama riseState check edilemiyordu.
                    yield return null;
                }
                

                if (Data.RiseState != RiseState.Falling)
                {
                    Data.RiseState = RiseState.None;
                    MediatorEventbus.ChainMotionEvents.OnStop?.Invoke();
                    UIEventbus.OnTowerHeightChange?.Invoke(Data.TargetHeight, Data.Id); //TODO: TEMP

                }
                
               // UIEventbus.OnTowerHeightChange?.Invoke(Data.TargetHeight, Data.Id); //TODO: TEMP
            }

            else if (Data.RiseState == RiseState.Falling)
            {
                startHeight = RoundByCustomUnit(Data.ActiveHolder.localPosition.y);

                while (Data.ActiveHolder.localPosition.y > Data.TargetHeight)
                {
                    Move(Data.ActiveHolder.localPosition);
                    
                    if ((Data.ActiveHolder.localPosition.y - (startHeight - unit)) <= tolerance)
                    {
                        if (Data.ActiveParts.Count == 0)
                        {
                            Data.RiseState = RiseState.None;
                            break;
                        }

                        startHeight -= unit;
                        LoseLastPart();
                    }

                    yield return null;
                }

                if (Data.RiseState != RiseState.Rising)
                {
                    Data.RiseState = RiseState.None;
                    //MediatorEventbus.ChainMotionEvents.OnStop?.Invoke();
                    UIEventbus.OnTowerHeightChange?.Invoke(Data.TargetHeight, Data.Id); //TODO: TEMP

                }
                
                //UIEventbus.OnTowerHeightChange?.Invoke(Data.TargetHeight, Data.Id); //TODO: TEMP
            }

            else {}

           
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
            pos.y = 0 - i * unit; //TODO: İlki hep sabit kalır (0 olacağından) ama diğerleri ters sıralanır, dikkat!
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