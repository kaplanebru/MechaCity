using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class MultiplayerSetter : MonoBehaviour
    {
        public bool isMultiplayerOn = true;
        public bool isTestingWithoutCombat = false;
        public  bool fasterCombat;


        public static bool IsMultiplayerOn;
        public static bool IsTestingWithoutCombat;
        public static bool FasterCombat;

        private void Awake()
        {
            IsMultiplayerOn = isMultiplayerOn;
            IsTestingWithoutCombat = isTestingWithoutCombat;
            FasterCombat = fasterCombat;
        }
    }
}

