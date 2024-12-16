using System;
using System.Collections;
using System.Collections.Generic;
using GameUI;
using UnityEngine;

namespace Health
{
    public class HealthHolder : MonoBehaviour, ITowerRelatedElement
{
    public HealthIcon iconPrefab;
    public List<HealthIcon> icons;
    private int _currentHealth;
    [SerializeField] public float gap = 0.3f;
    public int maxHealth = 3;
    [SerializeField] private Transform parent;

    public int Id { get; set; }
    public void Initialize(int id)
    {
        Id = id;
    }
    private void OnEnable()
    {
        CreateIcons();
    }
    
    void Activate(HealthIcon icon)
    {
        icon.gameObject.SetActive(true);
        activeIcons.Add(icon);
    }

    public float range = 5;
    float GetRandomAngle()
    {
        float random;
        do
        {
            random = UnityEngine.Random.Range(-range, range);
        } while (range == 0);

        return random;
    }

    Vector2 GetRandomEulers() => new Vector2(GetRandomAngle(), GetRandomAngle());
    

    public void AdjustIcons(int health)
    {
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

    private Vector2 _eulers;
    
    public void OrderIcons()
    {
        _eulers = GetRandomEulers();
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
        
        activeIcons.ForEach(i=>i.SetRotation(_eulers));
    }

    private List<HealthIcon> activeIcons = new();
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


    public void DisableAll()
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
}
