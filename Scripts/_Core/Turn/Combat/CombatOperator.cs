using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
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

    public class CombatOperator : IEnumeratorContainer
    {
        private readonly CombatData Data = new();
        private CombatPairController _pairController;
        private List<TowerData> _towers = new(); //todo: sadece visual tutulabilir

        private CombatTimingData _timingData;
        private bool isReversed = false;


        public void ReverseCombatDirection()
        {
            isReversed = !isReversed;
            // Debug.Log("combat reversed: " + isReversed);
        }

        public void SetElements(CombatTimingData combatTimingData, CombatPairController pairController)
        {
            _timingData = combatTimingData;
            _pairController = pairController;
        }

        public void Setup(List<uint> actors)
        {
            _towers.Clear();
            foreach (var actorID in actors)
            {
                var actor = ActorDB.Registry[actorID];
                _towers.AddRange(actor.Towers);
            }

            _towers?.ForEach(t => t.VisualData.ColorHandler.ToOriginalSelectionColor());
        }

        public void Fasten()
        {
            Data.AccelerateValues();
        }

        public void Fire()
        {
            GeneralEventbus.OnCoroutineTrigger?.Invoke(this);
        }


        void SetSelectionColor(uint mainActorID, bool select = true)
        {
            var mainActor = ActorDB.Registry[mainActorID];
            if (select)
            {
                foreach (var tower in mainActor.Towers)
                {
                    tower.VisualData.ColorHandler.ToSelectionColor();
                }

                GeneralEventbus.IndicatorEvents.OnActorHoverByCombat?.Invoke(mainActorID);
                Eventbus.CombatEvents.OnTurnTowerSelection?.Invoke(mainActorID);
            }
            else
            {
                foreach (var tower in mainActor.Towers)
                {
                    tower.VisualData.ColorHandler.ToOriginalSelectionColor();
                }

                GeneralEventbus.IndicatorEvents.OnActorLeftByCombat?.Invoke();
                Eventbus.CombatEvents.OnTurnTowerDeselect?.Invoke();
            }
        }

        uint[] GetActors()
        {
            return isReversed
                ? ActorDB.Registry.Keys.ToArray().Reverse().ToArray()
                : ActorDB.Registry.Keys.ToArray();
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

            var actors = GetActors();
            foreach (var actorID in actors)
            {
                Eventbus.CombatEvents.OnNextActor?.Invoke(Data.cursorDuration);
                yield return new WaitForSeconds(Data.cursorDuration);

                var pairs = _pairController.GetPairGroupByActorID(actorID);
                SetSelectionColor(actorID);

                yield return new WaitForSeconds(Data.selectionDelay);

                foreach (var pair in pairs)
                {
                    if (pair.Combat())
                    {
                        yield return new WaitUntil(() => pair.CombatCompleted);
                    }
                    else
                    {
                        yield return new WaitForSeconds(_timingData.skipDelay);
                        yield return new WaitForSeconds(Data.afterCombatDelay);
                        SetSelectionColor(actorID, false);
                    }
                }
            }

            Eventbus.CombatEvents.OnCombatEnding?.Invoke();
            yield return new WaitForSeconds(0.5f);
           
            EndCombat();
        }

        void EndCombat()
        {
            _pairController.ResetCombatCompletedForAll();
            BpEventbus.ActionEvents.OnRestoreSelectionAmount?.Invoke();
            Eventbus.CombatEvents.OnCombatTerminated?.Invoke();
            //not: aslında eventler ters sırayla çağrılmalı?!
            Unsubscribe();
        }

        void DeselectAlteredTowers()
        {
            _towers?.ForEach(t => t.VisualData.ColorHandler.ToOriginalSelectionColor());
        }

        public void Unsubscribe()
        {
            DeselectAlteredTowers();
        }
    }
}