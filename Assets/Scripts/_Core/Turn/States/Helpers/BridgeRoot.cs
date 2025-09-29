using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Enums;
using Towers;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BridgeRoot : MonoBehaviour, ITowerRelatedElement
{
    public int Id { get; set; }
    public Transform root;
    public float offset = 2;
    public int modelDirection = -1;
    
    private TowerObject mainTowerObject;
    private TowerObject targetTowerObject;
    private Vector3 direction;
    private float startScale;
    public BPTimingData timingData;

    public void Initialize(int id)
    {
        Id = id;
        mainTowerObject = AllTowers.GetTower(Id);
        startScale = root.transform.localScale.z;
    }

    public void Stretch(int targetId)
    {
        targetTowerObject = AllTowers.GetTower(targetId);
        var distance = Vector3.Distance(mainTowerObject.transform.position, targetTowerObject.transform.position);
        SetDirection();
        GetFacingDirection();
        
        root.transform.rotation = Quaternion.LookRotation(direction);
        distance = (distance - offset) * modelDirection;
        root.transform.DOScaleZ(distance, timingData.DurationByType[BpType.DoubleSelf]); //distance - offset
    }

    public void RemoveBridge()
    {
        if(root.gameObject.activeInHierarchy)
            root.transform.DOScaleZ(startScale, timingData.DurationByType[BpType.DoubleSelf]);
    }

    void SetDirection()
    {
        direction = (targetTowerObject.transform.position - mainTowerObject.transform.position).normalized;
    }

    public void Show(bool isShowing)
    {
        root.gameObject.SetActive(isShowing);
    }

    void GetFacingDirection()
    {
        float dotProduct = Vector3.Dot(root.transform.forward, direction);
        
        // if (dotProduct > 0) // Positive value
        // {
        //     Debug.Log("GameObject is facing the front of the target.");
        // }
        // else if (dotProduct < 0) // Negative value
        // {
        //     Debug.Log("GameObject is facing the back of the target.");
        // }
        // else // dotProduct is close to 0
        // {
        //     Debug.Log("GameObject is perpendicular to the target.");
        // }
    }

    // int GetSide()
    // {
    //     float dotProduct = Vector3.Dot(targetTower.transform.right, direction);
    //     return dotProduct > 0 ? 0 : 1;
    // }
}
