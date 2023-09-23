using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;

public class PopupTurnText : BaseInfoText
{
    protected override void SetInfoText(string teamName)
    {
        infoText.text = teamName + "'s Turn"; 
    }
}
