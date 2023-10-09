using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainMover : MonoBehaviour
{
    [SerializeField] private List<Transform> _objs = new();
    [SerializeField] private List<Vector3> _points = new();
    public float speed = 0.1f;

    private void OnEnable()
    {
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

    IEnumerator MoveRoutine()
    {
        yield return new WaitWhile(() => _points.Count == 0);
        for (int i = 0; i < _objs.Count; i++)
        {
            if (i == _objs.Count - 1)
            {
                _objs[i].transform.position = Vector3.Lerp(_objs[i].transform.position, _points[0], speed);
                // _chains[i].transform.rotation = Quaternion.Lerp(_chains[i].transform.rotation, _chains[0].transform.rotation, speed);

                i = 0;
                yield return new WaitForFixedUpdate();
                //continue;
            }

            _objs[i].transform.position = Vector3.Lerp(_objs[i].transform.position, _points[i + 1], speed);
            // _chains[i].transform.rotation = Quaternion.Lerp(_chains[i].transform.rotation, _chains[i + 1].transform.rotation, speed);
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnDisable()
    {
        ChainEvents.OnPointsCreated -= SetPoints;
        ChainEvents.OnObjsCreated -= SetObjs;
    }
}