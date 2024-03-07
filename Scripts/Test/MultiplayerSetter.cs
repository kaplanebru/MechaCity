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

        public static bool IsMultiplayerOn;
        public static bool IsTestingWithoutCombat;

        private void Awake()
        {
            IsMultiplayerOn = isMultiplayerOn;
            IsTestingWithoutCombat = isTestingWithoutCombat;
        }
    }
}

