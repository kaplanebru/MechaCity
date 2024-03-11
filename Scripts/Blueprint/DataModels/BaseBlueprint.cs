using System.Collections.Generic;
using Enums;

namespace Blueprint
{
    public abstract class BaseBlueprint
    {
        public abstract BpType Type { get; set; }

        public abstract int[] SelectedElements { get; set; }
        public abstract void TryTakeAction();

        public abstract void TryRestoreAction();
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