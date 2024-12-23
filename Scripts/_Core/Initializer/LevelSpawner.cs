using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private Transform allTowers;
    [SerializeField] Transform levelPrefab;
    [SerializeField] private PlayerAndTeamInitializer playerAndTeamInitializer;

    private void OnEnable()
    {
        InstantiateLevelPrefab();
    }

    void InstantiateLevelPrefab()
    {
        Instantiate(levelPrefab, allTowers);
    }

}
