using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float motionDistance = 1;
    public float duration = 1;
    
    private float hiddenPosY;

    public void Initialize()
    {
        hiddenPosY = transform.localPosition.y;
    }
    
    void RevealSelf() //eventle çalışmamalı bir sürü var
    {
        transform.DOLocalMoveY(motionDistance, duration);
    }

    void Hide()
    {
        transform.DOLocalMoveY(hiddenPosY, duration);
    }
}
