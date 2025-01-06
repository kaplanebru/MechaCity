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
    public float offset = 1;
    public float yOffset = 2.4f;
    
    private TowerObject mainTowerObject;
    private TowerObject targetTowerObject;
    private Vector3 direction;
    public void Initialize(int id)
    {
        Id = id;
        mainTowerObject = AllTowers.GetTower(Id);
    }

    public void Stretch(int targetId)
    {
        targetTowerObject = AllTowers.GetTower(targetId);
        var distance = Vector3.Distance(transform.position, targetTowerObject.transform.position);

        SetDirection();
        
        //var side = GetSide();
        transform.rotation = Quaternion.LookRotation(direction);
        root.transform.DOMoveY(root.transform.position.y + yOffset, .5f).OnComplete(() =>
        {
          
            root.transform.DOScaleZ(distance - offset, 1); //side
        });


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
