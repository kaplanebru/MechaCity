using System;
using System.Collections;
using System.Collections.Generic;
using GameUI;
using UnityEngine;

public class HealthIcon : MonoBehaviour
{
    public Transform iconPrefab;
    public List<Transform> icons;
    private int _currentHealth;
    [SerializeField] public float gap = 0.3f;
    public int maxHealth = 3;
    [SerializeField] private Transform parent;


    private void OnEnable()
    {
        CreateIcons();
        UIEventbus.OnHealthChange += AdjustIcons;
    }

    private void OnDisable()
    {
        UIEventbus.OnHealthChange -= AdjustIcons;
    }
    
    void Activate(Transform icon)
    {
        icon.gameObject.SetActive(true);
        activeIcons.Add(icon);
    }

    private void AdjustIcons(int health, GameObject towerGameObject)
    {
        if (towerGameObject != parent.gameObject) return;
        
        _currentHealth = health;
        OrderIcons();
    }
    
    public void CreateIcons() //int maxHealth
    {
        for (int i = 0; i < maxHealth; i++)
        {
            icons.Add(Instantiate(iconPrefab, transform));
        }
    }

    public void OrderIcons()
    {
        DisableAll();
        ResetAll();
        if(_currentHealth == 0) return;

        if (_currentHealth % 2 == 1)
        {
            OddOrder();
        }
        else
        {
            PairOrder();
        }
    }

    private List<Transform> activeIcons = new();
    void OddOrder()
    {
        int counter = 0;
        Activate(icons[0]);

        if(_currentHealth <= 1) return;
        
        for (int i = 1; i < _currentHealth; i++)
        {
            var icon = icons[i];
            Activate(icon);
            
            if (i % 2 == 1)
            {
                counter++;
                icon.transform.localPosition += new Vector3(counter * gap, 0, 0);
            }
            else
            {
                icon.transform.localPosition -= new Vector3(counter * gap, 0, 0);
            }
        }
    }

    
    void PairOrder()
    {
        int counter = 0;
        
        icons[0].transform.localPosition += Vector3.right * gap / 2;
        icons[1].transform.localPosition -= Vector3.right * gap / 2;
        
        Activate(icons[0]);
        Activate(icons[1]);

        if(_currentHealth <= 2) return;
        
        for (int i = 2; i < _currentHealth; i++)
        {
            var icon = icons[i];
            Activate(icon);

            if (i % 2 == 0)
            {
                counter++;
                icon.transform.localPosition += new Vector3((counter * gap) + gap/2, 0, 0);
            }
            else
            {
                icon.transform.localPosition -= new Vector3((counter * gap) + gap/2, 0, 0);
            }
        }
        
    }

    // void MoveIcons()
    // {
    //     foreach (var icon in activeIcons)
    //     {
    //         
    //     }
    // }

    void DisableAll()
    {
        icons.ForEach(i => i.gameObject.SetActive(false));
        activeIcons.Clear();
    }

    void ResetAll()
    {
        foreach (var icon in icons)
        {
            Vector3 pos = icons[0].transform.localPosition;
            pos.x = 0;
            icon.transform.localPosition = pos;
        }
    }
}