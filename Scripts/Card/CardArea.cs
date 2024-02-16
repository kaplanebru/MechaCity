using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class CardArea : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform origin;
    public Blueprint blueprintPrefab;
    public DataModels.BPHolder bpHolder;
    public List<Blueprint> cards = new List<Blueprint>();

    void OnDrawGizmos()
    {
        DrawPath();
    }

    void DrawPath()
    {
        if (waypoints == null || waypoints.Length < 2)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
        
        Gizmos.DrawSphere(origin.transform.position, .1f);
    }

    void CreateCards()
    {
        // for (var i = 0; i < waypoints.Length; i++)
        // {
        //     var point = waypoints[i];
        //     
        //     Blueprint blueprint = Instantiate(blueprintPrefab, origin); //transform
        //     blueprint.transform.localPosition += Vector3.up * (waypoints.Length-i) * 0.01f;
        //
        //     cards.Add(blueprint);
        //     blueprint.Data = bpHolder.BPData[i];
        //     blueprint.Setup();
        //     SetRotation(point.transform.position, cards.Last());
        // }
    }

    void SetRotation(Vector3 point, Blueprint blueprint)
    {
        var direction = (point - origin.transform.position).normalized;
       
        // Quaternion angledRot = Quaternion.LookRotation(direction);
        // card.transform.rotation = Quaternion.Euler(angledRot.x, angledRot.eulerAngles.y, 0);
        blueprint.transform.localRotation = Quaternion.LookRotation(direction);

    }

    private void Start()
    {
        CreateCards();
       //MoveOnPath();
    }

    void MoveOnPath()
    {
        if (waypoints.Length >= 2)
        {
            // Create a path tween using DOTween (movement along the path)
            transform.DOPath(GetWaypointsPositions(), 2f, PathType.Linear, PathMode.Full3D, 10, Color.green)
                .SetOptions(false)
                .SetLookAt(0.01f)
                .SetEase(Ease.Linear)
                .OnComplete(() => Debug.Log("Path animation completed."));
        }
    }

    Vector3[] GetWaypointsPositions()
    {
       
        Vector3[] positions = new Vector3[waypoints.Length];

        for (int i = 0; i < waypoints.Length; i++)
        {
            positions[i] = waypoints[i].position;
        }

        return positions;
    }
}
