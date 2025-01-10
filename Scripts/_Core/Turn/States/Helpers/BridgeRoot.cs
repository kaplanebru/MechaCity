using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Towers;
using UnityEngine;

public class BridgeRoot : MonoBehaviour, ITowerRelatedElement
{
    public int Id { get; set; }
    public Transform root;
    public float offset = 2;
    public int modelDirection = -1;
    public float yOffset = 2.4f;
    
    private TowerObject mainTowerObject;
    private TowerObject targetTowerObject;
    private Vector3 direction;
    private float startScale;
    public void Initialize(int id)
    {
        Id = id;
        mainTowerObject = AllTowers.GetTower(Id);
        startScale = root.transform.localScale.z;
    }

    public void Stretch(int targetId)
    {
        targetTowerObject = AllTowers.GetTower(targetId);
        var distance = Vector3.Distance(transform.position, targetTowerObject.transform.position);
        SetDirection();
        
        root.transform.rotation = Quaternion.LookRotation(direction);
        distance = (distance - offset) * modelDirection;
        //distance -= root.transform.localScale.z * 2; //temp
        root.transform.DOScaleZ(distance, 1); //distance - offset
    }

    public void RemoveBridge()
    {
        root.transform.DOScaleZ(startScale, 1);
    }

    void SetDirection()
    {
        direction = (targetTowerObject.transform.position - mainTowerObject.transform.position).normalized;
    }

    public void Show(bool isShowing)
    {
        root.gameObject.SetActive(isShowing);
    }

    // int GetSide()
    // {
    //     float dotProduct = Vector3.Dot(targetTower.transform.right, direction);
    //     return dotProduct > 0 ? 0 : 1;
    // }
}
