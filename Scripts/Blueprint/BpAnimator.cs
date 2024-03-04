using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BpAnimator : MonoBehaviour
{
    public Transform objectToSync; // The object you want to synchronize
    public Camera referenceCamera; // The camera you want to sync the position to
    public Transform model;

    void Start()
    {
        if (objectToSync != null && referenceCamera != null)
        {
            Vector3 viewportPosition = referenceCamera.WorldToViewportPoint(objectToSync.position);
            model.transform.position = Camera.main.ViewportToWorldPoint(viewportPosition);
            Quaternion relativeRotation = Quaternion.Inverse(referenceCamera.transform.rotation) * objectToSync.rotation;
            model.transform.rotation = Camera.main.transform.rotation * relativeRotation;
        }
    }
}
