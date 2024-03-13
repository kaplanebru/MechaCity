using System.Collections.Generic;
using Enums;

namespace Blueprint
{
    public abstract class BaseBlueprint
    {
        public abstract BpType Type { get; set; }

        public abstract int Lifespan {get; set; } //todo: lifespan da değişken olacak burda olmamalı
        public abstract void TryTakeAction(int[] selectedItems);

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