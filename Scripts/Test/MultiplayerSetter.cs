using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class MultiplayerSetter : MonoBehaviour
    {
        public bool isMultiplayerOn = true;

        public static bool IsMultiplayerOn;

        private void Awake()
        {
            IsMultiplayerOn = isMultiplayerOn;
        }
    }
}

