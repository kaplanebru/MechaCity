using Network;

namespace Blueprint
{
    public class BPSubscriber : Subscriber<BlueprintManager>
    {
        public BPSubscriber(BlueprintManager mainClass) : base(mainClass) {}
        
        public override void Subscribe()
        {
            BpEventbus.UIEvents.OnInteraction += MainClass.ChangeStateAndSetBp; //todo: Daha sonra, (datadaki değişkenleri ayırdıktan sonra) network obj olarak data gönderilir yaparız
            BpEventbus.OnSendingSelectionsForExecution += MainClass.SendBpExecutionRequestByUser;
            BpEventbus.OnDirectBpExecution += MainClass.TryExecuteBpBySystem;
            BpEventbus.LifespanEvents.OnRestore += MainClass.RestoreFromBp;
            BpEventbus.LifespanEvents.OnExpiredTracker += MainClass.RemoveExpiredBp;

            NetworkEventbus.ServerEvents.OnBpSelectionByServer += MainClass.SetCurrentBpByServer;
            NetworkEventbus.ServerEvents.OnBpExecutionRequestByServer += MainClass.TryExecuteBpBySystem;
            NetworkEventbus.ServerEvents.OnPlayerPersonaSet += MainClass.PlayerPersona.SetPlayerPersona;

            TurnStatusEvents.OnTurnEnding += MainClass.UpdateBpTrackers;
        }

        public override void Unsubscribe()
        {
            BpEventbus.OnSendingSelectionsForExecution -= MainClass.SendBpExecutionRequestByUser;
            BpEventbus.OnDirectBpExecution -= MainClass.TryExecuteBpBySystem;
            BpEventbus.UIEvents.OnInteraction -= MainClass.ChangeStateAndSetBp;
            BpEventbus.LifespanEvents.OnRestore -= MainClass.RestoreFromBp;
            BpEventbus.LifespanEvents.OnExpiredTracker -= MainClass.RemoveExpiredBp;

            NetworkEventbus.ServerEvents.OnBpSelectionByServer -= MainClass.SetCurrentBpByServer;
            NetworkEventbus.ServerEvents.OnBpExecutionRequestByServer -= MainClass.TryExecuteBpBySystem;
            NetworkEventbus.ServerEvents.OnPlayerPersonaSet -= MainClass.PlayerPersona.SetPlayerPersona;

            TurnStatusEvents.OnTurnEnding -= MainClass.UpdateBpTrackers;
        }

       
    }
}