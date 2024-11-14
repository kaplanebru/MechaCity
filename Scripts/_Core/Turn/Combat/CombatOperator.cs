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
        private List<TowerData> _towers = new();

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
                var actor = ActorHolder.Registry[actorID];
                _towers.AddRange(actor.Towers);
            }

            _towers?.ForEach(t => t.ColorHandler.ToOriginalColor());
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
                foreach (var tower in pair.MainActor.Towers)
                {
                    tower.ColorHandler.ToSelectionColor();
                }

                GeneralEventbus.IndicatorEvents.OnActorHoverByCombat?.Invoke(pair.MainActor.ID);
                Eventbus.CombatEvents.OnTurnTowerSelection?.Invoke(pair.MainActor.ID);
            }
            else
            {
                foreach (var tower in pair.MainActor.Towers)
                {
                    tower.ColorHandler.ToOriginalColor();
                }

                GeneralEventbus.IndicatorEvents.OnActorLeftByCombat?.Invoke();
                Eventbus.CombatEvents.OnTurnTowerDeselect?.Invoke();
            }
        }

        uint[] GetActors()
        {
            return isReversed
                ? ActorHolder.Registry.Keys.ToArray().Reverse().ToArray()
                : ActorHolder.Registry.Keys.ToArray();
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

                var pairs = _pairController.GetPairByActorID(actorID);
                pairs.ForEach(p => SetSelectionColor(p));

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
                        SetSelectionColor(pair, false);
                    }
                }
            }

            Eventbus.CombatEvents.OnCombatEnding?.Invoke();
            yield return new WaitForSeconds(0.5f);
            AllTowers.RestoreBullets();

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
            _towers?.ForEach(t => t.ColorHandler.ToOriginalColor());
        }

        public void Unsubscribe()
        {
            DeselectAlteredTowers();
        }
    }
}