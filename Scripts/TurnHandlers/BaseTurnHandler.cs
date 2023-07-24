using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTurnHandler : MonoBehaviour
{
    public abstract void Subscribe();
    public abstract void Unsubscribe();
    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }
}
