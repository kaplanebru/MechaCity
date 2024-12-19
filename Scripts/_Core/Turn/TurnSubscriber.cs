using GameUI;
using Network;

namespace Turn
{
    public class TurnSubscriber : Subscriber<TurnManager>
    {
        public TurnSubscriber(TurnManager mainClass) : base(mainClass) {}
        public override void Subscribe()
        {
            MainClass.BpEventHandler.SubscribeToBlueprintEvents();
            MainClass.StateHolder.Setup();
            
            Eventbus.CombatEvents.OnPairsSet += MainClass.SendCombatPairs;
            TeamEvents.OnTeamsSet += MainClass.SetTurnTeams;
            NetworkEventbus.OnAllClientsSet += MainClass.FirstTurn;
            NetworkEventbus.ServerEvents.OnStateChangeRequestByServer += MainClass.ChangeStateBySystem;
            
            Eventbus.CombatEvents.OnCombatTerminated += MainClass.EndTurn;
            UIEventbus.OnApplyPossibility += MainClass.HighlightButtonRequest; //todo: sadece state'i tutan bir kod olabilir, state'e göre action alan
            UIEventbus.OnButtonClicked += MainClass.StateEndByUser;

            BpEventbus.StateEvents.OnDirectStateChangeFromIntruder += MainClass.GetPreviousState;
            BpEventbus.StateEvents.StateChangeRequestToIntruder += MainClass.SendStateChangeRequest;
            MainClass.PairController.Subscribe();
            MainClass.TurnHelper.Subscribe();
        }

        public override void Unsubscribe()
        {
            MainClass.BpEventHandler.UnsubscribeFromBlueprintEvents();
            MainClass.StateHolder.UnsubscribeFromConstantEvents(); 
            
            Eventbus.CombatEvents.OnPairsSet -= MainClass.SendCombatPairs;
            TeamEvents.OnTeamsSet -= MainClass.SetTurnTeams;
            NetworkEventbus.OnAllClientsSet -= MainClass.FirstTurn;
            NetworkEventbus.ServerEvents.OnStateChangeRequestByServer -= MainClass.ChangeStateBySystem;

            Eventbus.CombatEvents.OnCombatTerminated -= MainClass.EndTurn; //TODO: check
            UIEventbus.OnApplyPossibility -= MainClass.HighlightButtonRequest;
            UIEventbus.OnButtonClicked -= MainClass.StateEndByUser;

            BpEventbus.StateEvents.OnDirectStateChangeFromIntruder -= MainClass.GetPreviousState;
            BpEventbus.StateEvents.StateChangeRequestToIntruder -= MainClass.SendStateChangeRequest;
            MainClass.PairController.Unsubscribe();
            MainClass.TurnHelper.Unsubscribe();
        }
    }
}