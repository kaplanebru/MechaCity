using System.Collections.Generic;
using Actor;
using Enums;
using Enums.Selections;
using JetBrains.Annotations;
using Towers;

namespace Blueprint
{
    public abstract class BaseBlueprint
    {
        public abstract BpType Type { get; set; }
        public bool IsPlaying { get; set; }
        public BPTimingData TimingData;

        public abstract SelectionType SelectionType { get; set; }

        public virtual int Level
        {
            set => Lifespan = value; //duruma göre value+1 override
        }

        public abstract int Lifespan {get; set; } //todo: lifespan da değişken olacak burda olmamalı
        
        public abstract bool TryTakeAction([CanBeNull] uint[] selectedItems);

        public void CompleteAction()
        {
            IsPlaying = false;
            BpEventbus.ActionEvents.OnBpActionCompleted?.Invoke(Type);
            DeselectItems();
        }

        public async void CompleteActionWithDelay()
        {
            var delay = TimingData.DurationByType[Type];
            if(delay > 0)
                await DelayMaker.WaitForSeconds(delay);
            
            CompleteAction();
        }

        public void SetTimingData(BPTimingData timingData)=> TimingData = timingData;
        

        //public abstract void CheckSelectionConstraints(int[] selectedItems);

        public abstract void TryRestoreAction(uint selectedItem);

        public void DeselectItems() //[CanBeNull] uint[] selectedItems
        {
            // foreach (var actorID in selectedItems)
            // {
            //     ActorHolder.Registry[actorID].Towers.
            // }
            
            AllTowers.ResetTowerColors();
        }
       
        
    }
    
    public interface IBpActionProcessor<out TAction> where TAction : IBpAction
    {
        public TAction BpAction { get; }
        public BpType Type { get; set; }
    }


    public interface IBpAction
    {
        public BpType BPType { get; set; }
        public void Execute(params object[] obj);
    }
}