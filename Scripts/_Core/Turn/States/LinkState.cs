using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using GameUI;
using Network;
using Towers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Turn
{
    [Serializable]
    public class TowerGroupTransferData : BaseTurnTransferData
    {
        public override TurnStateType StateType { get; set; } = TurnStateType.Link;
        public override List<uint> Actors { get; set; } = new();
    }

    public class LinkState : BaseTurnState, ITransferDataHolder<TowerGroupTransferData>
    {
        public TowerGroupTransferData TransferData { get; private set; } = new();
        public override TurnStateType StateType => TurnStateType.Link;
        public override int StateId { get; set; }


        private ILinkOperator currentLinkOperator;
        private ILinkOperator[] linkOperators = new ILinkOperator[2];
        
        private LinkOperator linkOperator = new();
        private DoubleLinkOperator doubleLinkOperator = new(); //double ölene kadar ömrü var

        public override void SubscribeToConstantEvents()
        {
            Eventbus.LinkEvents.OnFloorsOpened += LinkTowers;
            BpEventbus.ActionEvents.OnDoubleSelfAction += GetDoubles;
            
            SetLinkOperators();
        }

        void SetLinkOperators()
        {
            linkOperators[0] = linkOperator;
            linkOperators[1] = doubleLinkOperator;

            currentLinkOperator = linkOperators[0];
        }
        

        public override void Subscribe()
        {
            Eventbus.LinkEvents.OnLinkStateBegin?.Invoke();
        }
        
        
        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data) //(params object[] args)
        {
            TransferData.Actors = data.Actors;
            
            SetLinkOperatorAndSubscribe();
            currentLinkOperator.SetTowers(TransferData.Towers.ToArray()); //y
           

            if (currentLinkOperator.Type == LinkOperatorType.Double)
                TransferData.Towers = doubleLinkOperator.setter.SetTransferData().ToList(); //yo
            
            Eventbus.LinkEvents.OnLinkLoading?.Invoke(TransferData.Towers);
        }

        private void GetDoubles(DoubleTower doubleTower)
        {
            AllDoubles.Add(doubleTower);
        }

        private void SetLinkOperatorAndSubscribe()
        {
            foreach (var tower in TransferData.Towers)
            {
                if (AllDoubles.InspectTower(tower))
                {
                    currentLinkOperator = doubleLinkOperator;
                 
                    goto Subscribe;
                }
            }
           
            currentLinkOperator = linkOperator;
            
            
            Subscribe:
            Debug.Log(currentLinkOperator.Type);
            NetworkEventbus.InputEvents.OnObjectClicked += currentLinkOperator.TowerSelected;
        }
       
        private void SwitchLinkOperator(LinkOperatorType type)
        {
            currentLinkOperator = linkOperators.First(o => o.Type == type);
        }
        
        private void LinkTowers()
        {
            AllTowers.DisableClickability();
            Eventbus.LinkEvents.OnLinkingTowers?.Invoke(TransferData.Towers);
            MediatorEventbus.ChainLinkEvents.OnLinkedTowers?.Invoke(TransferData.Towers.ToArray());
        }
        
        public override void Unsubscribe()
        {
            Eventbus.LinkEvents.OnUnlink?.Invoke(TransferData.Towers);
            MediatorEventbus.ChainLinkEvents.OnLinkBroken?.Invoke();

            NetworkEventbus.InputEvents.OnObjectClicked -= currentLinkOperator.TowerSelected;
            AllTowers.EnableClickability();
            
            SelectionEvents.OnSelectionTerminated?.Invoke();
        }

        public override void UnsubscribeFromConstantEvents()
        {
            Eventbus.LinkEvents.OnFloorsOpened -= LinkTowers;
            BpEventbus.ActionEvents.OnDoubleSelfAction -= GetDoubles;

        }
    }
}