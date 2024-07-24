using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

public class FloorGroups : MonoBehaviour
{
    [SerializeField]private Floor[] group;
    public float duration = 0.5f;
    public float openSize = 0.4f;

    private List<Floor> selectedFloors = new();

    private void OnEnable()
    {
        GeneralEventbus.OnTowersAndTeamsReady += Initialize;
        Eventbus.LinkEvents.OnLinkLoading += OpenFloors;
        Eventbus.LinkEvents.OnUnlink += ResetFloors;
    }
    
    private void Initialize()
    {
        group = FindObjectsOfType<Floor>(); //Todo daha sonra getcomp ile de yapılır
    }

    private void OpenFloors(List<int> ids)
    {
        foreach (var id in ids)
        {
            var floor = group.FirstOrDefault(f => f.Id == id);
            selectedFloors.Add(floor);
            floor.Open(openSize, duration);
        }

        Invoke(nameof(FloorsOpenedCall), duration);
    }

    void FloorsOpenedCall()
    {
        Eventbus.LinkEvents.OnFloorsOpened?.Invoke();
    }
    
    private void ResetFloors(List<int> ids)
    {
        foreach (var floor in selectedFloors)
        {
            floor.RestoreHeight(duration);
        }
    }
    
    private void OnDisable()
    {
        GeneralEventbus.OnTowersAndTeamsReady -= Initialize;
        Eventbus.LinkEvents.OnLinkLoading -= OpenFloors;
        Eventbus.LinkEvents.OnUnlink -= ResetFloors;
    }


}