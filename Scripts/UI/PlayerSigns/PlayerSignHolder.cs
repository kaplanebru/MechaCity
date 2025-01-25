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
            UIEventbus.OnPlayerSet += SetPlayerSign;
            NetworkEventbus.OnAllClientsSet += SetOtherPlayerSign;
        }

      

        private void SetPlayerSign(string playerName, TeamType teamType)
        {
            signs[0].Setup(playerName, teamType);
            // foreach (var sign in signs)
            // {
            //     if(sign.isSet) continue;
            //     
            //     sign.Setup(playerName);
            //     return;
            // }
        }

        private void SetOtherPlayerSign(object[] args)
        {
            var teamNamesByType = args[0] as Dictionary<TeamType, string>;
            
            //var otherTeam = teamNamesByType[]
            
        }
        
        private void OnDisable()
        {
            UIEventbus.OnPlayerSet -= SetPlayerSign;
            NetworkEventbus.OnAllClientsSet -= SetOtherPlayerSign;
        }
    }

}
