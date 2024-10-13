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
            BpEventbus.ActionEvents.OnDoubleSelfAction += GetDoubles;

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

            SetLinkOperatorAndSubscribe();
            currentLinkOperator.SetTowers(TransferData.Actors); //y


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
            currentLinkOperator = TransferData.Actors.Any(a => ActorHolder.Registry[a].Type == ActorType.MultiTower)
                ? linkOperators[ActorType.MultiTower]
                : linkOperators[ActorType.Standard];

            Debug.Log(currentLinkOperator.Type);
            NetworkEventbus.InputEvents.OnObjectClicked += currentLinkOperator.TowerSelected;
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