using System;
using System.Collections.Generic;
using System.Linq;
using Actor;
using DG.Tweening;
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
            SetLinkOperators();
        }

        void SetLinkOperators()
        {
            if(!linkOperators.ContainsKey(ActorType.Standard))
                linkOperators.Add(ActorType.Standard, new LinkOperator());
            if(!linkOperators.ContainsKey(ActorType.MultiTower))
                linkOperators.Add(ActorType.MultiTower, new DoubleLinkOperator());
            
            currentLinkOperator = linkOperators[ActorType.Standard];
        }
        
        public override void Subscribe() {}


        private uint[] activeActors;
        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data) //(params object[] args)
        {
            Debug.Log("link state process");
            TransferData.Actors = data.Actors;
            activeActors = ActorDB.GetActiveActors(TransferData.Actors.ToArray()).ToArray();
            TransferData.towers = ActorDB.ResolveTowersFromActors(activeActors).ToList();

            if (activeActors.Length <= 1)
            {
                EndStateManually();
                return;
            }
            CheckInterruptions();
            SetLinkOperatorAndSubscribe();
            currentLinkOperator.SetTowers(activeActors);
            Eventbus.LinkEvents.OnLinkLoading?.Invoke(TransferData.towers);
        }

        void CheckInterruptions()
        {
            Eventbus.LinkEvents.OnLinkActorsLoaded?.Invoke(activeActors.ToList());
        }

        private void SetLinkOperatorAndSubscribe()
        {
            currentLinkOperator = activeActors.Any(a => ActorDB.Registry[a].Type == ActorType.MultiTower)
                ? linkOperators[ActorType.MultiTower]
                : linkOperators[ActorType.Standard];
            
            
            NetworkEventbus.InputEvents.OnObjectClicked += currentLinkOperator.TowerSelected;
            MediatorEventbus.ChainLinkEvents.OnFloorsOpened += EnableLinkMotion;

        }
        
        private void EnableLinkMotion(int[] ids = null)
        {
            AllTowers.DisableClickability();
            
            if(activeActors.Length <= 1) return;
            Eventbus.LinkEvents.OnLinkMotionEnabled?.Invoke(TransferData.towers);
        }

        public override void Unsubscribe()
        {
            Eventbus.LinkEvents.OnUnlink?.Invoke(TransferData.towers);
            MediatorEventbus.ChainLinkEvents.OnLinkBroken?.Invoke();

            NetworkEventbus.InputEvents.OnObjectClicked -= currentLinkOperator.TowerSelected;
            MediatorEventbus.ChainLinkEvents.OnFloorsOpened -= EnableLinkMotion;
            
            AllTowers.EnableClickability();
            SelectionEvents.OnSelectionTerminated?.Invoke();
        }

        async void EndStateManually()
        {
            UIEventbus.OnPopupTime?.Invoke(PopupType.NoLink);

            await DelayMaker.WaitForSeconds(1);
            AllTowers.EnableClickability();
            SelectionEvents.OnSelectionTerminated?.Invoke();
            UIEventbus.OnButtonClicked?.Invoke();
        }

        public override void UnsubscribeFromConstantEvents()
        {
            //Eventbus.LinkEvents.OnFloorsOpened -= LinkTowers;
        }
    }
}