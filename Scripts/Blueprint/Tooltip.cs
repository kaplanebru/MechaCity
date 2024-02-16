using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public GameObject content;

    private void OnEnable()
    {
        content.SetActive(false);
    }

    private void OnMouseEnter()
    {
        content.SetActive(true);
    }

    private void OnMouseExit()
    {
        content.SetActive(false);
    }
}