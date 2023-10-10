using System.Collections;
using System.Collections.Generic;
using GenericHelper;
using UnityEngine;

public class LinkPool : Pool<Transform>
{
    private void Awake()
    {
        Instance = this;
    }
}
