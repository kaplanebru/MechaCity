using System.Collections;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSign : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isSet = false;
    [SerializeField] private TextMeshProUGUI textHolder;
    [SerializeField] private Image imageHolder; //todo: later
    [SerializeField] private GameObject statsGo;

    private string _playerName;
    public TeamType teamType;

    public void Setup(string playerName, TeamType type)
    {
        _playerName = playerName;
        teamType = type;
        isSet = true;
      
        SetText();
    }

    void SetText()
    {
        textHolder.text = _playerName;
    }

    void ShowStats()
    {
        statsGo.SetActive(true);
    }

    void HidesStats()
    {
        statsGo.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowStats();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HidesStats();
    }
}