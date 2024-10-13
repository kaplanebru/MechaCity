using System;
using System.Collections.Generic;
using System.Linq;
using Actor;
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
        public List<int> towers = new();
    }

    public class LinkState : BaseTurnState, ITransferDataHolder<TowerGroupTransferData>
    {
        public TowerGroupTransferData TransferData { get; private set; } = new();
        public override TurnStateType StateType => TurnStateType.Link;
        public override int StateId { get; set; }
        
        private ILinkOperator currentLinkOperator;
        private Dictionary<ActorType, ILinkOperator> linkOperators = new();
        
        public override void SubscribeToConstantEvents()
        {
            Eventbus.LinkEvents.OnFloorsOpened += LinkTowers;
            SetLinkOperators();
        }

        void SetLinkOperators()
        {
            linkOperators.Add(ActorType.Standard, new LinkOperator());
            linkOperators.Add(ActorType.MultiTower, new DoubleLinkOperator());
            
            currentLinkOperator = linkOperators[ActorType.Standard];
        }
        
        public override void Subscribe()
        {
            Eventbus.LinkEvents.OnLinkStateBegin?.Invoke();
        }


        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data) //(params object[] args)
        {
            TransferData.Actors = data.Actors;
            TransferData.towers = ActorHolder.ResolveTowersFromActors(TransferData.Actors.ToArray());
            
            SetLinkOperatorAndSubscribe();
            currentLinkOperator.SetTowers(TransferData.Actors.ToArray());
            
            Eventbus.LinkEvents.OnLinkLoading?.Invoke(TransferData.towers);
        }
        

        private void SetLinkOperatorAndSubscribe()
        {
            currentLinkOperator = TransferData.Actors.Any(a => ActorHolder.Registry[a].Type == ActorType.MultiTower)
                ? linkOperators[ActorType.MultiTower]
                : linkOperators[ActorType.Standard];
            
            NetworkEventbus.InputEvents.OnObjectClicked += currentLinkOperator.TowerSelected;
        }
        

        private void LinkTowers()
        {
            AllTowers.DisableClickability();
            Eventbus.LinkEvents.OnLinkingTowers?.Invoke(TransferData.towers);
            MediatorEventbus.ChainLinkEvents.OnLinkedTowers?.Invoke(TransferData.towers.ToArray());
        }

        public override void Unsubscribe()
        {
            Eventbus.LinkEvents.OnUnlink?.Invoke(TransferData.towers);
            MediatorEventbus.ChainLinkEvents.OnLinkBroken?.Invoke();

            NetworkEventbus.InputEvents.OnObjectClicked -= currentLinkOperator.TowerSelected;
            AllTowers.EnableClickability();

            SelectionEvents.OnSelectionTerminated?.Invoke();
        }

        public override void UnsubscribeFromConstantEvents()
        {
            Eventbus.LinkEvents.OnFloorsOpened -= LinkTowers;
        }
    }
}