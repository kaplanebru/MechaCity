using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Network;
using UnityEngine;

namespace GameUI
{
    public class PlayerSignHolder : MonoBehaviour
    {
        [SerializeField] private PlayerSign[] signs;

        private void OnEnable()
        {
            NetworkEventbus.UIEvents.OnPlayerSet += SetPlayerSign;
            NetworkEventbus.OnAllClientsSet += SetOtherPlayerSign;
        }
        private void SetPlayerSign(string playerName, TeamType teamType)
        {
            signs[0].Setup(playerName, teamType);
        }

        private void SetOtherPlayerSign(object[] args)
        {
            var teamNamesByType = args[0] as Dictionary<TeamType, string>;

            foreach (var teamNameByType in teamNamesByType)
            {
                if(teamNameByType.Key == signs[0].teamType) continue;
                signs[1].Setup(teamNameByType.Value, teamNameByType.Key);
            }
        }
        
        private void OnDisable()
        {
            NetworkEventbus.UIEvents.OnPlayerSet -= SetPlayerSign;
            NetworkEventbus.OnAllClientsSet -= SetOtherPlayerSign;
        }
    }

}
