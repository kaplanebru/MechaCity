using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class TurnButton : MonoBehaviour
{
    public TurnStateType turnStateType;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Color[] buttonColors;
    
    public void Highlight(bool shine)
    {
        buttonImage.color = shine ? buttonColors[1] : buttonColors[0];
    }
}
