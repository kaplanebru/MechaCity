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
    public float speed = 0.025f;


   
    public class MotionData
    {
        public bool IsRising;
        public int TargetHeight;
        public int StepAmount;
    }
    private void OnEnable()
    {
        //targetheight güncellemesini dinleyebilir
        //coroutine link state'de başlatılır, state sonu kapatılır
    }

    private void Start()
    {
        DisableAll();
        StartCoroutine(MoveRoutine(0));
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            targetHeight++;
        }
    }

    IEnumerator MoveRoutine(int height)
    {
        targetHeight = height;
        int step = 0;
        
        while (true)
        {
            while (Mathf.Abs(activeHolder.localPosition.y - targetHeight) > 0.001f) // "<" yapmadım lowering ise diye
            {
                Vector3 pos = activeHolder.localPosition;
                pos.y = Mathf.MoveTowards(pos.y, targetHeight, speed);
                activeHolder.localPosition = pos;

                if (pos.y >= step)
                {
                    step++;
                    if (passiveParts.Count == 0) yield break;
                    GetNextPart();
                }
                yield return null;
            }

            activeHolder.localPosition = new Vector3(activeHolder.localPosition.x, targetHeight, activeHolder.localPosition.z);
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
