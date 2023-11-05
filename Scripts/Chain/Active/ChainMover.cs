using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using MyNamespace;
using UnityEngine;

public class ChainMover : MonoBehaviour
{
    public ChainData Data;

    [SerializeField] private List<ChainLink> _links = new();
    [SerializeField] private List<Vector3> _points = new();
    private List<Quaternion> _rotations = new();


    public static float LinearSpeed = 0;
    public static float MachinerySpeed;

    private float _rotationExtentPerLink;

    private void OnEnable()
    {
        Data = GetComponent<ChainSpawner>().Data;
        MachinerySpeed = Data.MachinerySpeed;

        enabled = Data.IsMoving;

        ChainEvents.OnLinksCreated += SetLinksAndPoints;
        ChainEvents.OnCogSpeedSet += GetTotalCogSpeed;
    }


    private int totalCogTeeth = 0;
    private int _toothSize;
    private float toothUnits;

    private int counter = 0;

    private IEnumerator Start()
    {
        yield return new WaitWhile(() => _points.Count == 0 || LinearSpeed == 0);
        MoveChain();
    }

    private void GetTotalCogSpeed(int teethAmount, float toothUnit)
    {
        totalCogTeeth += teethAmount;
        toothUnits += toothUnit;
        counter++;
        
        if (counter != Data.CogAmount) return;
        counter = 0;
        SetSpeed();
    }


    void SetLinksAndPoints(List<ChainLink> links, List<Vector3> points)
    {
        _links = links;
        _points = points;
    }

    void SetSpeed()
    {
        LinearSpeed = Data.MachinerySpeed /
                      (totalCogTeeth +
                       (Data.Unit - toothUnits / Data.CogAmount) *
                       totalCogTeeth); //fazlalığı da cogteethe eklemiş oluyoruz
        //BOŞLUK + TEETH SİZE   /(totalCogTeeth + (Data.Unit - (teethInterval + _toothSize)) * totalCogTeeth);
//        print("chain speed: " + LinearSpeed);
    }


    void RotatePointsByObj()
    {
        foreach (var link in _links)
        {
            _rotations.Add(link.transform.rotation);
        }
    }

    void MoveChain()
    {
        print(_links.Count);
        RotatePointsByObj();

        for (int i = 0; i < _links.Count; i++)
        {
            StartCoroutine(LinkMotionRoutine(i));
        }
    }

    IEnumerator LinkMotionRoutine(int startIndex)
    {
        float speed = Data.SetMotionByGear ? LinearSpeed : Data.SpeedMultiplier;
        _rotationExtentPerLink = speed * Data.LinkRotationMultiplier;
        int j = startIndex;

        while (true)
        {
            switch (Data.motionDirection)
            {
                case ChainEnums.ChainDirection.Clockwise:
                    j++;
                    j %= _points.Count;
                    break;
                case ChainEnums.ChainDirection.ReverseClock:
                    j--;
                    if (j < 0)
                        j = _points.Count - 1;
                    break;
            }

            while (Vector3.Distance(_links[startIndex].transform.position, _points[j]) > 0.05f) //0.1f
            {
                _links[startIndex].transform.position = Vector3.MoveTowards(
                    _links[startIndex].transform.position,
                    _points[j], speed);

                _links[startIndex].transform.rotation = Quaternion.Slerp(
                    _links[startIndex].transform.rotation,
                    _rotations[j], _rotationExtentPerLink);

                yield return new WaitForFixedUpdate();
            }

            _links[startIndex].transform.position = _points[j];
            //_links[startIndex].transform.rotation = _rotations[j];
        }
    }

    private void OnDisable()
    {
        ChainEvents.OnLinksCreated -= SetLinksAndPoints;
        ChainEvents.OnCogSpeedSet -= GetTotalCogSpeed;
    }
}