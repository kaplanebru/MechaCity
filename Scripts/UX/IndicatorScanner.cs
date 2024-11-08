using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScanner : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        //SendRay();
    }

    void SendRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity,LayerMask.GetMask("Clickable")))
        {
            Debug.Log("Hit object: " + hit.collider.name);
        }
    }
    
   
}
