using System.Collections;
using System.Collections.Generic;
using Enums;
using Network;
using UnityEngine;

namespace GameUI
{
    public class PlayerText : BaseInfoText
    {
      
       

        public override void SetInfoText(string teamName)
        {
            InfoText.text = teamName;
        }
        
    }

}
