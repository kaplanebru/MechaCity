using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Towers;
using Unity.Mathematics;
using UnityEngine;

public class DirectionCursorSpawner : MonoBehaviour
{
    public DirectionCursor cursorPrefab;
    public List<DirectionCursor> directionCursors;

    private void OnEnable()
    {
        TowerEvents.OnTowersCreated += Initialize;
    }

    public void Initialize()
    {
        CreateCursors();
        SetPositions();
    }

    void CreateCursors()
    {
        for (int i = 0; i < AllTowers.TowersCount; i++)
        {
            directionCursors.Add(Instantiate(cursorPrefab));
            directionCursors.Last().id = i;
        }
    }

    void SetPositions()
    {
        for (int i = 0; i < AllTowers.TowersCount; i++)
        {
            var cursor = directionCursors[i];
            var pos1 = AllTowers.GetTower(i).transform.position;
            var pos2 = AllTowers.GetTower((i + 1) % AllTowers.TowersCount).transform.position;
            
            cursor.transform.position = (pos1 + pos2) / 2;
            cursor.transform.rotation = Quaternion.LookRotation((pos2 - pos1).normalized);
            cursor.transform.position += Vector3.up * 0.5f; //temp
        }
    }

    private void OnDisable()
    {
        TowerEvents.OnTowersCreated -= Initialize;
    }

    //o zaman cursorların towerlardan haberi olmalı
    //TODO: target ölüyse cursor'ın rengi solar, target vurulamazsa da rengi değişir
}
