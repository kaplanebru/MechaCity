using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace UI
{
    public class PopupTurnText : BaseInfoText
    {
        protected override void SetInfoText(string teamName)
        {
            infoText.text = teamName + "'s Turn"; 
        }
    }

}
