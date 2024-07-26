using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chain;
using UnityEngine;

namespace ChainInGame
{
    public class InGameChainHandler : MonoBehaviour
    {
        public Machinery[] machineries;
        public List<Cogwheel> gears = new();

        private List<MachineryInGame> _machineriesInGame = new();

        private MachineryInGame _currentMachineryInGame;

        private void OnEnable()
        {
            MediatorEventbus.SetupEvents.OnGearsReady += GetGears;
            ChainEvents.InGameEvents.OnOptionSet += SelectMachinery;

            MediatorEventbus.ChainTurnEvents.OnLinkedTowers += ShowMachinery;
            MediatorEventbus.ChainTurnEvents.OnLinkBroken += ResetMachinery;
            
            MediatorEventbus.ChainTurnEvents.OnRising += MoveWithChain;
            MediatorEventbus.ChainTurnEvents.OnStop += StopMotion;
        }

        private void GetGears(IGear[] iGear)
        {
            SetMachinery();
            foreach (var gear in iGear)
            {
                gears.Add(gear as Cogwheel);
            }
        }

        void SetMachinery()
        {
            if (machineries.Length == 0)
                machineries = FindObjectsOfType<Machinery>();

            SetInGameMachineries();
            ChainEvents.InGameEvents.OnMachineriesSet?.Invoke(machineries);


            _currentMachineryInGame = _machineriesInGame.First();
        }

        void SetInGameMachineries()
        {
            foreach (var machinery in machineries)
            {
                _machineriesInGame.Add(new MachineryInGame(machinery));
            }
        }


        private void MoveWithChain()
        {
            _currentMachineryInGame.StartMotion();
        }


        private void ResetMachinery()
        {
            _currentMachineryInGame.EmptyMachinery();
        }

        private void ShowMachinery(int[] ids)
        {
            foreach (var id in ids)
            {
                var gear = gears.FirstOrDefault(g => g.id == id);
                if (gear != null)
                {
                    _currentMachineryInGame.AddToMachinery(gear);
                }
            }
        }

        void SelectMachinery(int i)
        {
            _currentMachineryInGame = _machineriesInGame[i];
        }

        public void StopMotion()
        {
            _currentMachineryInGame.StopMotion();
        }

        public void StartMotion()
        {
            _currentMachineryInGame.StartMotion();
        }

        private void OnDisable()
        {
            MediatorEventbus.SetupEvents.OnGearsReady -= GetGears;
            ChainEvents.InGameEvents.OnOptionSet -= SelectMachinery;

            MediatorEventbus.ChainTurnEvents.OnLinkedTowers -= ShowMachinery;
            MediatorEventbus.ChainTurnEvents.OnLinkBroken -= ResetMachinery;
            
            MediatorEventbus.ChainTurnEvents.OnRising -= MoveWithChain;
            MediatorEventbus.ChainTurnEvents.OnStop -= StopMotion;
        }

        #region AvecInput

        // void CreateInteractables()
        // {
        //     foreach (var gear in gears)
        //     {
        //         var interactable = gear.gameObject.AddComponent<Interactable>();
        //         
        //         int id = gear.GetComponentInChildren<GearIdentifier>().id; //temp
        //         
        //         interactable.Setup(gear, id);
        //         interactable.gameObject.layer = LayerMask.NameToLayer("InteractableGear");
        //     }
        // }

        Ray RayFromCamera() => Camera.main.ScreenPointToRay(Input.mousePosition);

        // private void Update()
        // {
        //     ControlInputs();
        // }

        // void ControlInputs()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //     
        //         if (Physics.Raycast(RayFromCamera(), out RaycastHit hit, LayerMask.GetMask("InteractableGear")))
        //         {
        //             var interactable = hit.transform.gameObject.GetComponent<Interactable>();
        //             if(!interactable) return;
        //             _currentMachineryInGame.AddToMachinery(interactable);
        //         }
        //     }
        //
        //     if (Input.GetMouseButtonDown(1))
        //     {
        //         if (Physics.Raycast(RayFromCamera(), out RaycastHit hit, LayerMask.GetMask("InteractableGear")))
        //         {
        //             var interactable = hit.transform.gameObject.GetComponent<Interactable>();
        //             if(!interactable) return;
        //             _currentMachineryInGame.RemoveFromMachinery(interactable);
        //         }
        //     }
        // }

        #endregion
    }
}