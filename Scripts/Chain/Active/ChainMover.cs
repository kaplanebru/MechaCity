using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using Enums;
using UnityEngine;

public class ChainMover : MonoBehaviour
{
    public ChainData Data;
    private List<Transform> _links = new();
    private List<Vector3> _points = new();
    private List<Quaternion> _rotations = new();
    public float cogSpeed = 30f;
    public static float CogSpeed;

    private void OnEnable()
    {
        CogSpeed = cogSpeed;
        ChainEvents.OnMotionStateSet += enable => enabled = enable;
        ChainEvents.OnPointsCreated += SetPoints;
        ChainEvents.OnLinksCreated += SetLinks;
    }

    void SetPoints(List<Vector3> points)
    {
        _points = points;
    }

    void SetLinks(List<Transform> links)
    {
        _links = links;
        StartCoroutine(nameof(MoveRoutine));
    }

   

    void RotatePointsByObj()
    {
        foreach (var link in _links)
        {
            _rotations.Add(link.transform.rotation);
        }
    }

    IEnumerator MoveRoutine()
    {
        yield return new WaitWhile(() => _points.Count == 0);
        RotatePointsByObj();

        for (int i = 0; i < _links.Count; i++)
        {
            StartCoroutine(LinkRoutine(i));
        }
        
    }

    IEnumerator LinkRoutine(int startIndex)
    {
        int j = startIndex;
        while (true)
        {
            switch (Data.motionDirection)
            {
                case ChainDirection.Clockwise:
                    j++;
                    j %= _points.Count;
                    break;
                case ChainDirection.ReverseClock:
                    j--;
                    if (j < 0)
                        j = _points.Count-1;
                    break;
            }
           

            while (Vector3.Distance(_links[startIndex].transform.position, _points[j]) > 0.05f) //0.1f
            {
                _links[startIndex].transform.position = Vector3.MoveTowards(
                    _links[startIndex].transform.position,
                    _points[j], Data.LinearSpeed);
                
                _links[startIndex].transform.rotation = Quaternion.Slerp(
                    _links[startIndex].transform.rotation,
                    _rotations[j], Data.LinkRotationExtent);

                yield return new WaitForFixedUpdate();
            }

            _links[startIndex].transform.position = _points[j];
            //_links[startIndex].transform.rotation = _rotations[j];
        }
    }

    private void OnDisable()
    {
        ChainEvents.OnMotionStateSet -= enable => enabled = enable;
        ChainEvents.OnPointsCreated -= SetPoints;
        ChainEvents.OnLinksCreated -= SetLinks;
    }
}