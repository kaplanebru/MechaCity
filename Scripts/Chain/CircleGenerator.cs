using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGenerator
{
    float _radius = 1;
    int _userAmount = 6;
    //public Transform destination;

    private int CircleAmount => _userAmount - _userAmount % 6;

    private int _totalAmount;
    private int _rotationAngle = 0;

    [SerializeField] List<Vector3> _chainPoints = new();

    public CircleGenerator(float radius, int userAmount)
    {
        _radius = radius;
        _userAmount = userAmount;
    }

    Vector3 CirclePoint(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);

        return new Vector3(x, 0, y) * _radius; // + transform.position;
    }

    void GetCirclePoints()
    {
        //SetRotationAngle();
        float baseAngle = 360f / CircleAmount;

        for (int i = 0; i <= CircleAmount; i++)
        {
            var newAngle = (baseAngle * i + _rotationAngle) % 360; //print(newAngle);
            _chainPoints.Add(CirclePoint(newAngle));
        }

        InsertIntersectionPoints();
    }

    void InsertIntersectionPoints()
    {
        _chainPoints.Insert(_chainPoints.Count / 2, _chainPoints[_chainPoints.Count / 2]);
        _chainPoints.Insert(_chainPoints.Count - 1, _chainPoints[0]);
        _totalAmount = CircleAmount + 2;
    }

    // void SetRotationAngle()
    // {
    //     Vector3 direction;
    //     direction = (destination.transform.position - transform.position).normalized;
    //     rotationAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
    // }
}