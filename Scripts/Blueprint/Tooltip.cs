using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public GameObject content;

    private void OnEnable()
    {
        Disable();
    }

    private void OnMouseEnter()
    {
        Enable();
    }

    private void OnMouseDown()
    {
        Disable();
    }

    private void OnMouseExit()
    {
        Disable();
    }

    void Enable()
    {
        content.SetActive(true);
    }

    void Disable()
    {
        content.SetActive(false);
    }
}