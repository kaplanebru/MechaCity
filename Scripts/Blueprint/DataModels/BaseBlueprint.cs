using System.Collections.Generic;
using Enums;
using Enums.Selections;

namespace Blueprint
{
    public abstract class BaseBlueprint
    {
        public abstract BpType Type { get; set; }

        public abstract SelectionType SelectionType { get; set; }

        public virtual int Level
        {
            
            set => Lifespan = value; //duruma göre value+1 override
        }

        public abstract int Lifespan {get; set; } //todo: lifespan da değişken olacak burda olmamalı
        
        public abstract int MaxSelectionAmount { get; set; }
        public abstract bool TryTakeAction(int[] selectedItems);

        //public abstract void CheckSelectionConstraints(int[] selectedItems);

        public abstract void TryRestoreAction(int selectedItem);
    }
    
    public interface IBpActionProcessor<out TAction> where TAction : IBpAction
    {
        public TAction BpAction { get; }
        public BpType Type { get; set; }
    }


    public interface IBpAction
    {
        public void Execute(params object[] obj);
    }
}