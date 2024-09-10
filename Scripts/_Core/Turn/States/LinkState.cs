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
        public override List<int> Towers { get; set; } = new();
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
            Eventbus.LinkEvents.OnDoubleSelfAction += SwitchLinkOperator;
            
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
            NetworkEventbus.InputEvents.OnObjectClicked += currentLinkOperator.TowerSelected;
            Eventbus.LinkEvents.OnLinkStateBegin?.Invoke();
        }
        
        
        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data) //(params object[] args)
        {
            TransferData.Towers = data.Towers;
            currentLinkOperator.GetTowers(TransferData.Towers.ToArray());
            
            if (currentLinkOperator.Type == LinkOperatorType.Double)
                TransferData.Towers = doubleLinkOperator.SetDoublesClickable();

            Eventbus.LinkEvents.OnLinkLoading?.Invoke(TransferData.Towers);
        }

        private int[] doubles;
        private void SwitchLinkOperator(LinkOperatorType type, int[] towers = null)
        {
            currentLinkOperator = linkOperators.First(o => o.Type == type);
            
            doubles = towers;
            if (currentLinkOperator.Type == LinkOperatorType.Double)
                doubleLinkOperator.GetDoubles(doubles.ToList());
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
        }

        public override void UnsubscribeFromConstantEvents()
        {
            Eventbus.LinkEvents.OnFloorsOpened -= LinkTowers;
            Eventbus.LinkEvents.OnDoubleSelfAction -= SwitchLinkOperator;
        }
    }
}