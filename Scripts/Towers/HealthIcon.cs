using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIcon : MonoBehaviour
{
    public Transform iconPrefab;
    public List<Transform> icons;
    public int currentHealth;
    [SerializeField] public float gap = 0.3f;
    public int maxHealth = 3;
    public int startHealth = 3;


    private void Start()
    {
        CreateIcon();
    }

    public void CreateIcon() //int maxHealth
    {
        for (int i = 0; i < maxHealth; i++)
        {
            icons.Add(Instantiate(iconPrefab, transform));
        }
        OrderIcons();
    }

    public void OrderIcons()
    {
        DisableAll();
        ResetAll();
        if(currentHealth == 0) return;

        if (currentHealth % 2 == 1)
        {
            OddOrder();
        }
        else
        {
            PairOrder();
        }
    }

    void OddOrder()
    {
        int counter = 0;
        icons[0].gameObject.SetActive(true);
        
        if(currentHealth <= 1) return;
        
        for (int i = 1; i < currentHealth; i++)
        {
            var icon = icons[i];
            icon.gameObject.SetActive(true);
            
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
        icons[0].gameObject.SetActive(true);
        icons[1].gameObject.SetActive(true);
        
        if(currentHealth <= 2) return;
        
        for (int i = 2; i < currentHealth; i++)
        {
            var icon = icons[i];
            icon.gameObject.SetActive(true);
            
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

    void DisableAll()
    {
        icons.ForEach(i => i.gameObject.SetActive(false));
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