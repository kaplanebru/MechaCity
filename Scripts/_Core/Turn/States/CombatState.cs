using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using Enums;
using Towers;
using Unity.Collections;
using UnityEngine;

namespace Turn
{
    public class CombatTransferData : BaseTurnTransferData // = sıfırlanacak data
    {
        public List<int> AlteredTowers = new();
    }

    
    [Serializable]
    public class CombatData
    {
        public List<CombatPair> CombatPairs = new();

        [ReadOnly] public float afterCombatDelay = .3f;
        public float selectionDelay = 0.3f;
        public float cursorDuration = 0.5f;
    }

    public class CombatState : BaseTurnState, ITurnTransferHandler<CombatTransferData>
    {
        public CombatTransferData TransferData { get; private set; } = new();
        //public CombatTimingData timingData;
        private readonly CombatData Data = new();
        private CombatPairListCreator combatPairListCreator; 

        public override TurnStateType StateType => TurnStateType.Combat;
        public override int StateId { get; set; }

       

        public override void Subscribe() {}
        

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
        {
            var incomingData = (TowerGroupTransferData) data;
            TransferData.AlteredTowers = incomingData.TowerGroup;
        }

        public void ConstantSetup()
        {
            combatPairListCreator = new CombatPairListCreator(Data.CombatPairs);
            combatPairListCreator.CreateCombatPairs(AllTowers.TowerDatas.ToList());
        }

        public override void StartState()
        {
           
            //Data.CreateReverseCombatPairs(AllTowers.TowerDatas.ToList(), true);
            
            TransferData.AlteredTowers.ForEach(at => AllTowers.GetTower(at).ResetColor());
            //StartCoroutine(nameof(FireRoutine)); //TODO: FİX LATER
            turnManager.StartCoroutine(this.FireRoutine());
        }

        void Select(CombatPair pair, bool select = true)
        {
            if (select)
                AllTowers.GetTower(pair.MainTowerData.UniqID).SelectColor();
            else
                AllTowers.GetTower(pair.MainTowerData.UniqID).ResetColor();
        }

        IEnumerator FireRoutine()
        {
            Debug.Log("coroutine started");
            Eventbus.CombatEvents.OnCombatReady?.Invoke();
            yield return new WaitForSeconds(turnManager.timingData.cameraDelay);
            Eventbus.CombatEvents.OnCombatStarted?.Invoke();


            for (int i = 0; i < AllTowers.TowersCount; i++)
            {
                var pair = Data.CombatPairs[i];
                Select(pair);

                yield return new WaitForSeconds(Data.selectionDelay);


                pair.Combat(turnManager.timingData);

                yield return new WaitUntil(() => pair.CombatCompleted);
                yield return new WaitForSeconds(Data.afterCombatDelay);

                Eventbus.CombatEvents.OnFire?.Invoke(Data.cursorDuration);
                yield return new WaitForSeconds(Data.cursorDuration);
                Select(pair, false);
            }

            Eventbus.CombatEvents.OnCombatEnding?.Invoke();
            yield return new WaitForSeconds(0.5f);
            AllTowers.RestoreBullets();
            Eventbus.CombatEvents.OnCombatTerminated?.Invoke();
            CompleteState();
            turnManager.SwitchState(StateId + 1);
        }

        void DeselectAlteredTowers()
        {
            TransferData.AlteredTowers?.ForEach(t =>
                AllTowers.GetTower(t).towerParts.SetColor(AllTowers.GetTower(t).Data.TeamTowerData.DefaultMaterial));
        }

        public override void ResetPreviousTurnData()
        {
            TransferData.AlteredTowers.Clear();
        }

        public override void Unsubscribe()
        {
            DeselectAlteredTowers();
        }
    }
}