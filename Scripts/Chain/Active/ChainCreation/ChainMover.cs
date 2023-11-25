using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chain;
using UnityEngine;

public interface Mover
{
    public float MachinerySpeed { get; set; }
    public int MachineryId { get; set; }

    public void MachinerySetup(float machinerySpeed, int machineryId, IMachinePartData data)
    {
    }
}

public class ChainMover : MonoBehaviour, Mover
{
    public float MachinerySpeed { get; set; }
    public int MachineryId { get; set; }

    public ChainData Data;

    [SerializeField] private List<ChainLink> _links = new();
    [SerializeField] private List<Vector3> _points = new();
    private List<Quaternion> _rotations = new();


    public float LinearSpeed = 0;
    private float _rotationExtentPerLink;

    private void OnEnable()
    {
        ChainEvents.OnCogSpeedSet += GetTotalCogSpeed;
    }


    private int totalCogTeeth = 0;
    private int _toothSize;
    private float toothIntervals;
    private int counter = 0;

    public void MachinerySetup(float machinerySpeed, int machineryId, IMachinePartData machinePartData)
    {
        MachinerySpeed = machinerySpeed;
        MachineryId = machineryId;
        Data = machinePartData as ChainData;
    }

    public void Setup(List<ChainLink> links)
    {
        _links = links;
    }

   


    public IEnumerator StartMover()
    {
        //Data =
        //_links = 
        //linear speedin 0dan büyük olması beklenebilir
        // enabled = Data.IsMoving;
        if (!Data.IsMoving) yield break;
      
        
        yield return new WaitUntil(() => _speedSet); //possbile bug: linear speed daha önceden set edilmiş olabilir.
        _speedSet = false;
        MoveChain();
    }


    private void GetTotalCogSpeed(int teethAmount, float toothInterval, int machineryId)
    {
        if (MachineryId != machineryId) return;

        totalCogTeeth += teethAmount;
        toothIntervals += toothInterval;
        counter++;

        if (counter != Data.CogAmount) return;
        counter = 0;
        SetSpeed();
    }


    private bool _speedSet = false;
    void SetSpeed()
    {
        var surplus = (Data.LinkInterval - (toothIntervals / Data.CogAmount)) * _links.Count; 
        //var surplus = (Data.LinkInterval - (toothIntervals / Data.CogAmount)) * totalCogTeeth; 
        // LinearSpeed = MachinerySpeed / (totalCogTeeth + surplus); 

        LinearSpeed = MachinerySpeed / Data.CogAmount / (_links.Count + surplus);
        print("speed: " + LinearSpeed + " SURPLUS: " + surplus);
        print("tooth interval: " +toothIntervals / Data.CogAmount);
        print(toothIntervals/ Data.CogAmount);
        print(Data.LinkInterval);
       
        _speedSet = true;
    }


    void GetRotationPoints()
    {
        _points.Clear();
        _rotations.Clear();
        foreach (var link in _links)
        {
            _rotations.Add(link.transform.rotation);
            _points.Add(link.transform.position);
        }
    }

    void MoveChain()
    {
        GetRotationPoints();

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
                    print(j);
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
        //ChainEvents.OnLinksCreated -= GetLinksAndPoints;
        ChainEvents.OnCogSpeedSet -= GetTotalCogSpeed;
    }
}


// public void GetLinksAndPoints(List<ChainLink> links, List<Vector3> points)
// {
//     print("get links event");
//
//     _links = links;
//     // _points = points; // yön değiştirince tekrar tekrar generate etmeyelim diye iptal
// }
