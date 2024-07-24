using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayHelper : MonoBehaviour
{
    public static DelayHelper Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Invoker(string text, float delay)
    {
        Invoke(text, delay);
    }

}
