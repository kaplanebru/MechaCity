using System.Collections;
using System.Collections.Generic;
using DataModels;
using Testing;
using Towers;
using Unity.Collections;
using UnityEngine;

namespace Turn
{
    public class CombatData
    {

        [ReadOnly] public float afterCombatDelay = .3f;
        public float selectionDelay = 0.3f;
        public float cursorDuration = 0.5f;

        public float accelerant = 10;

        public void AccelerateValues()
        {
            afterCombatDelay /= accelerant;
            selectionDelay /= accelerant;
            cursorDuration /= accelerant;
        }
    }

    public class CombatHelper : IEnumeratorContainer
    {
        private readonly CombatData Data = new();
        private CombatPairController _pairController;
        private List<int> _towers;

        private CombatTimingData _timingData;
        

        public void GetElements(CombatTimingData combatTimingData, CombatPairController pairController)
        {
            _timingData = combatTimingData;
            _pairController = pairController;
        }

        public void Subscribe(List<int> towers)
        {
            _towers = towers;
            _towers?.ForEach(at => AllTowers.GetData(at).ColorHandler.ToOriginalColor());
        }

        public void Fasten()
        {
            Data.AccelerateValues();
        }

        public void Fire()
        {
            GeneralEventbus.OnCoroutineTrigger?.Invoke(this);
        }

        
        void SetSelectionColor(CombatPair pair, bool select = true)
        {
            if (select)
            {
                pair.MainTowerData.ColorHandler.ToSelectionColor();
                Eventbus.CombatEvents.OnTurnTowerSelection?.Invoke(pair.MainTowerData.UniqID);
            }
            else
            {
                pair.MainTowerData.ColorHandler.ToOriginalColor();
                Eventbus.CombatEvents.OnTurnTowerDeselect?.Invoke();
            }
        }

        public IEnumerator LeCoroutine()
        {
            if (MultiplayerSetter.IsTestingWithoutCombat)
            {
                yield return new WaitForSeconds(.5f);
                EndCombat();
                yield break;
            }

            Eventbus.CombatEvents.OnCombatReady?.Invoke();
            Eventbus.CombatEvents.OnCombatStarted?.Invoke();
            yield return new WaitForSeconds(_timingData.cameraDelay);

            for (int i = 0; i < AllTowers.TowersCount; i++)
            {
                Eventbus.CombatEvents.OnNextTower?.Invoke(Data.cursorDuration);
                yield return new WaitForSeconds(Data.cursorDuration);

                var pair = _pairController.GetPairByIndex(i);//Data.CombatPairs[i];
                SetSelectionColor(pair);

                yield return new WaitForSeconds(Data.selectionDelay);
                
                if (pair.Combat())
                {
                    yield return new WaitUntil(() => pair.CombatCompleted);
                }
                else
                {
                    yield return new WaitForSeconds(_timingData.skipDelay);
                    yield return new WaitForSeconds(Data.afterCombatDelay);
                    SetSelectionColor(pair, false);
                }
            }

            Eventbus.CombatEvents.OnCombatEnding?.Invoke();
            yield return new WaitForSeconds(0.5f);
            AllTowers.RestoreBullets();

            EndCombat();
        }

        void EndCombat()
        {
            _pairController.CombatPairs.ForEach(p=> p.CombatCompleted = false);
            
            Eventbus.CombatEvents.OnCombatTerminated?.Invoke();
            Unsubscribe();
        }

        void DeselectAlteredTowers()
        {
            _towers?.ForEach(t => AllTowers.GetData(t).ColorHandler.ToOriginalColor());
        }

        public void Unsubscribe()
        {
            DeselectAlteredTowers();
            BpEventbus.ActionEvents.OnRestoreSelectionAmount?.Invoke();
        }
    }
}