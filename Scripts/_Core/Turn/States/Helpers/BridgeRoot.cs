using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Towers;
using UnityEngine;

public class BridgeRoot : MonoBehaviour, ITowerRelatedElement
{
    public int Id { get; set; }
    public Transform[] roots;
    public float offset = 1;
    public float yOffset = 2.4f;
    
    private Tower mainTower;
    private Tower targetTower;
    private Vector3 direction;
    public void Initialize(int id)
    {
        Id = id;
        mainTower = AllTowers.GetTower(Id);
    }

    public void Stretch(int targetId)
    {
        targetTower = AllTowers.GetTower(targetId);
        var distance = Vector3.Distance(transform.position, targetTower.transform.position);

        SetDirection();
        
        //var side = GetSide();
        transform.rotation = Quaternion.LookRotation(direction);
        roots[0].transform.DOMoveY(roots[0].transform.position.y + yOffset, .5f).OnComplete(() =>
        {
          
            roots[0].transform.DOScaleZ(distance - offset, 1); //side
        });


    }

    void SetDirection()
    {
        direction = (targetTower.transform.position - mainTower.transform.position).normalized;
    }

    public void Show(bool isShowing)
    {
        foreach (var root in roots)
        {
            root.gameObject.SetActive(isShowing);
        }
    }

    // int GetSide()
    // {
    //     float dotProduct = Vector3.Dot(targetTower.transform.right, direction);
    //     return dotProduct > 0 ? 0 : 1;
    // }
}
