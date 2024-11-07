using Network;

namespace Blueprint
{
    public class BPSubscriber
    {
        private BlueprintManager Manager;
        public BPSubscriber(BlueprintManager manager)
        {
            Manager = manager;
        }
        internal void Subscribe()
        {
            BpEventbus.UIEvents.OnInteraction += Manager.ChangeStateAndSetBp; //todo: Daha sonra, (datadaki değişkenleri ayırdıktan sonra) network obj olarak data gönderilir yaparız
            BpEventbus.OnSendingSelectionsForExecution += Manager.SendBpExecutionRequestByUser;
            BpEventbus.OnDirectBpExecution += Manager.TryExecuteBpBySystem;
            BpEventbus.LifespanEvents.OnRestore += Manager.RestoreFromBp;
            BpEventbus.LifespanEvents.OnExpiredTracker += Manager.RemoveExpiredBp;

            NetworkEventbus.ServerEvents.OnBpSelectionByServer += Manager.SetCurrentBpByServer;
            NetworkEventbus.ServerEvents.OnBpExecutionRequestByServer += Manager.TryExecuteBpBySystem;
            NetworkEventbus.ServerEvents.OnPlayerPersonaSet += Manager.PlayerPersona.SetPlayerPersona;

            TurnStatusEvents.OnTurnEnding += Manager.UpdateBpTrackers;
        }

        internal void Unsubscribe()
        {
            BpEventbus.OnSendingSelectionsForExecution -= Manager.SendBpExecutionRequestByUser;
            BpEventbus.OnDirectBpExecution -= Manager.TryExecuteBpBySystem;
            BpEventbus.UIEvents.OnInteraction -= Manager.ChangeStateAndSetBp;
            BpEventbus.LifespanEvents.OnRestore -= Manager.RestoreFromBp;
            BpEventbus.LifespanEvents.OnExpiredTracker -= Manager.RemoveExpiredBp;

            NetworkEventbus.ServerEvents.OnBpSelectionByServer -= Manager.SetCurrentBpByServer;
            NetworkEventbus.ServerEvents.OnBpExecutionRequestByServer -= Manager.TryExecuteBpBySystem;
            NetworkEventbus.ServerEvents.OnPlayerPersonaSet -= Manager.PlayerPersona.SetPlayerPersona;

            TurnStatusEvents.OnTurnEnding -= Manager.UpdateBpTrackers;
        }
    }
}