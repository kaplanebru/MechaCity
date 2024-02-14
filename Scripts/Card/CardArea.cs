using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CardArea : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform origin;
    public Card cardPrefab;
    public List<Card> cards = new List<Card>();

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
        foreach (var point in waypoints)
        {
            cards.Add(Instantiate(cardPrefab, point));
            SetRotation(point.transform.position, cards.Last());
        }
    }

    void SetRotation(Vector3 point, Card card)
    {
        var direction = (point - origin.transform.position).normalized;
        card.transform.rotation = Quaternion.LookRotation(direction);
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
