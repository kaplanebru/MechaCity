using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SwitchTeamEffect : MonoBehaviour
{
    Material _regenMat;
    private void Start()
    {
        _regenMat = GetComponent<MeshRenderer>().material;
        Regenerate();
    }

    void Regenerate()
    {
        _regenMat.DOColor(Color.green, 10);
       // _regenMat.DOFade( Color.green, 2);
    }
}
