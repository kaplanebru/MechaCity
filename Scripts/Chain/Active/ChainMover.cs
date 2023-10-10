using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainMover : MonoBehaviour
{
    [SerializeField] private List<Transform> _links = new();
    [SerializeField] private List<Vector3> _points = new();
    private List<Quaternion> _rotations = new();
    public float speed = 0.1f;

    private void OnEnable()
    {
        ChainEvents.OnMotionDecision += enable => enabled = enable;
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
            j++;
            j %=_points.Count;

            while (Vector3.Distance(_links[startIndex].transform.position, _points[j]) > 0.1f)
            {
                _links[startIndex].transform.position = Vector3.Lerp(_links[startIndex].transform.position, _points[j], speed);
                _links[startIndex].transform.rotation =
                    Quaternion.Lerp(_links[startIndex].transform.rotation,
                        _rotations[j], speed);
                
                yield return new WaitForFixedUpdate();
            }
            
        }
    }

    private void OnDisable()
    {
        ChainEvents.OnMotionDecision -= enable => enabled = enable;
        ChainEvents.OnPointsCreated -= SetPoints;
        ChainEvents.OnLinksCreated -= SetLinks;
    }
}