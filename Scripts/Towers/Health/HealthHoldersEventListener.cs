using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthHoldersEventListener : MonoBehaviour
{
    public HealthHolder[] healthHolders;
    private void OnEnable()
    {
        GeneralEventbus.OnTowersCreated += GetHealthHolders;
        GeneralEventbus.OnHealthIconChangeRequest += AdjustHealthIcon;
    }

    private void GetHealthHolders()
    {
        healthHolders = GetComponentsInChildren<HealthHolder>(); //todo: bunalr level prefabından da halledilebilir
        print(healthHolders.Length);
    }

    private void AdjustHealthIcon(int health, int id)
    {
        var healthHolder = healthHolders.FirstOrDefault(h => h.Id == id);
        healthHolder.AdjustIcons(health);
    }

    private void OnDisable()
    {
        GeneralEventbus.OnTowersCreated -= GetHealthHolders;
        GeneralEventbus.OnHealthIconChangeRequest -= AdjustHealthIcon;
    }
}