using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructorTesterObject : MonoBehaviour
{
    private ConstructorTester tester = new();

    private void OnEnable()
    {
        tester.TestMe();
    }

    void Start()
    {
        
    }
}
