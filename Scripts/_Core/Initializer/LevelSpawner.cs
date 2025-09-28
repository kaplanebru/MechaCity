using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Unity.Netcode;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private Transform allTowers;
    [SerializeField] Transform levelPrefab;
    [SerializeField] private NetworkManager networkManager;

    private void OnEnable()
    {
        Initialize();
    }

    void Initialize()
    {
        InstantiateLevelPrefab();
        Invoke(nameof(InstantiateNetworkManager), .3f);
    }

    void InstantiateLevelPrefab()
    {
        Instantiate(levelPrefab, allTowers);
    }

    void InstantiateNetworkManager()
    {
        Instantiate(networkManager);

    }

}
