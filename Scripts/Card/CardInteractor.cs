using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInteractor : MonoBehaviour
{
    public Camera cardCam;
    private Ray ray;
    
    void Start()
    {
        StartCoroutine(nameof(InteractRoutine));
    }

    IEnumerator InteractRoutine()
    {
        while (true)
        {
            ray = cardCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, LayerMask.GetMask("Card")))
            {
                
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void Hover()
    {
        
    }
    
}
