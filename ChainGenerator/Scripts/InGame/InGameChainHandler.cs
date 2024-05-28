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
        public List<Cogwheel> gears;
        
        private List<MachineryInGame> _machineriesInGame = new();

        private MachineryInGame _currentMachineryInGame;

        private void OnEnable()
        {
            ChainEvents.InGameEvents.OnOptionSet += SelectMachinery;
            
            CommunEventbus.ChainTurnEvents.OnLinkedTowers += FillMachinery;
            CommunEventbus.ChainTurnEvents.OnLinkBroken += ResetMachinery;
            CommunEventbus.ChainTurnEvents.OnRising += MoveWithRise;
            
            
        }

        private void MoveWithRise(float duration)
        {
            _currentMachineryInGame.StartMotion();
            Invoke(nameof(StopMotion),duration);
        }
        

        private void ResetMachinery()
        {
            _currentMachineryInGame.EmptyMachinery();
        }

        private void FillMachinery(int[] ids)
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

        void Setup()
        {
            if(machineries.Length == 0)
                machineries = FindObjectsOfType<Machinery>();
            
            SetInGameMachineries();
            ChainEvents.InGameEvents.OnMachineriesSet?.Invoke(machineries);

            
            _currentMachineryInGame = _machineriesInGame.First();

            if (gears.Count == 0)
            {
                var spawnedGears = FindObjectsOfType<Cogwheel>(); //TODO: TEMP
                foreach (var gear in spawnedGears)
                {
                    if (gear.transform.CompareTag("Cosmetic"))
                    {
                        continue;
                    }
                    gears.Add(gear);
                }
            }
            
            
          


        }

        private void Start()
        {
            Setup();
        }
        
        void SetInGameMachineries()
        {
            foreach (var machinery in machineries)
            {
                _machineriesInGame.Add(new MachineryInGame(machinery));
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
            ChainEvents.InGameEvents.OnOptionSet -= SelectMachinery;
            
            CommunEventbus.ChainTurnEvents.OnLinkedTowers -= FillMachinery;
            CommunEventbus.ChainTurnEvents.OnLinkBroken -= ResetMachinery;
            CommunEventbus.ChainTurnEvents.OnRising -= MoveWithRise;
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

