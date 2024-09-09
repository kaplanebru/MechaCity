using System;
using System.Collections.Generic;
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
        
        
        private LinkOperator linkOperator = new();

        public override void SubscribeToConstantEvents()
        {
            Eventbus.LinkEvents.OnFloorsOpened += LinkTowers;

        }
        
        public override void Subscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked += linkOperator.TowerSelected;
            Eventbus.LinkEvents.OnLinkStateBegin?.Invoke();
        }
        
        
        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data) //(params object[] args)
        {
            TransferData.Towers = data.Towers;
            linkOperator.GetTowers(TransferData.Towers.ToArray());
            
            Eventbus.LinkEvents.OnLinkLoading?.Invoke(TransferData.Towers);
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
            NetworkEventbus.InputEvents.OnObjectClicked -= linkOperator.TowerSelected;
            AllTowers.EnableClickability();
        }

        public override void UnsubscribeFromConstantEvents()
        {
            Eventbus.LinkEvents.OnFloorsOpened -= LinkTowers;
        }
    }
}