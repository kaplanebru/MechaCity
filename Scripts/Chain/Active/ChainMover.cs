using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainMover : MonoBehaviour
{
    [SerializeField] private List<Transform> _objs = new();
    [SerializeField] private List<Vector3> _points = new();
    private List<Quaternion> _rotations = new();
    public float speed = 0.1f;

    private void OnEnable()
    {
        ChainEvents.OnMotionDecision += enable => enabled = enable;
        ChainEvents.OnPointsCreated += SetPoints;
        ChainEvents.OnObjsCreated += SetObjs;
    }

    void SetPoints(List<Vector3> points)
    {
        _points = points;
    }
    void SetObjs(List<Transform> objs)
    {
        _objs = objs;
        StartCoroutine(nameof(MoveRoutine));
    }

    void RotatePointsByObj()
    {
        foreach (var obj in _objs)
        {
            _rotations.Add(obj.transform.rotation);
        }
    }
    IEnumerator MoveRoutine()
    {
        yield return new WaitWhile(() => _points.Count == 0);
        RotatePointsByObj();

        for (int i = 0; i < _objs.Count; i++)
        {
            StartCoroutine(SingleChainRoutine(i));
        }
    }

    IEnumerator SingleChainRoutine(int startIndex)
    {
        int j = startIndex;
        while (true)
        {
            j++;
            j %=_points.Count;

            while (Vector3.Distance(_objs[startIndex].transform.position, _points[j]) > 0.1f)
            {
                _objs[startIndex].transform.position = Vector3.Lerp(_objs[startIndex].transform.position, _points[j], speed);
                _objs[startIndex].transform.rotation =
                    Quaternion.Lerp(_objs[startIndex].transform.rotation,
                        _rotations[j], speed);
                
                yield return new WaitForFixedUpdate();
            }
            
        }
    }

    private void OnDisable()
    {
        ChainEvents.OnMotionDecision -= enable => enabled = enable;
        ChainEvents.OnPointsCreated -= SetPoints;
        ChainEvents.OnObjsCreated -= SetObjs;
    }
}