using System.Collections;
using System.Collections.Generic;
using Chain;
using MyNamespace;
using UnityEngine;

public class ChainMover : MonoBehaviour
{
    public ChainData Data;
    
    private List<Transform> _links = new();
    private List<Vector3> _points = new();
    private List<Quaternion> _rotations = new();
    
    public float cogSpeed = 30f;
    public static float CogSpeed;
    public static float LinearSpeed;
    
    private float _rotationExtentPerLink;

    private void OnEnable()
    {
        CogSpeed = cogSpeed;
        ChainEvents.OnMotionStateSet += enable => enabled = enable;
        ChainEvents.OnPointsCreated += SetPoints;
        ChainEvents.OnLinksCreated += SetLinks;
        ChainEvents.OnCogSpeedSet += GetTotalCogSpeed;
    }


   
    private int totalCogTeeth = 0;
    private int counter = 0;

    private float teethInterval;
    private void GetTotalCogSpeed(int teethAmount, float interval)
    {
        counter++;
        totalCogTeeth += teethAmount;
        teethInterval = interval;
      
        // print(ChainSpawner.ArcCount); //TODO bug detection
        // if (counter == ChainSpawner.ArcCount)
        // {
        //     LinearSpeed = CogSpeed / (totalCogTeeth + (Data.Unit - interval) * totalCogTeeth);//(Data.Unit / interval)); fazlalığı da cogteethe eklemiş oluyoruz
        //     //print("linear speed: " + LinearSpeed);
        //     StartCoroutine(nameof(MoveRoutine));
        //     
        //     counter = 0;
        //     totalCogTeeth = 0;
        // }
    }

    void SetPoints(List<Vector3> points)
    {
        _points = points;
    }

    void SetLinks(List<Transform> links)
    {
        _links = links;
        LinearSpeed = CogSpeed / (totalCogTeeth + (Data.Unit - teethInterval) * totalCogTeeth);//(Data.Unit / interval)); fazlalığı da cogteethe eklemiş oluyoruz
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
            StartCoroutine(LinkMotionRoutine(i));
        }
        
    }

    IEnumerator LinkMotionRoutine(int startIndex)
    {
        float speed = Data.SetMotionByGear ? LinearSpeed : Data.Speed;
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
                        j = _points.Count-1;
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
        ChainEvents.OnMotionStateSet -= enable => enabled = enable;
        ChainEvents.OnPointsCreated -= SetPoints;
        ChainEvents.OnLinksCreated -= SetLinks;
    }
}