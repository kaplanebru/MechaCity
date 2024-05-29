using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blueprint
{
    public class SelectionIncrementAction : IBpAction
    {
        public void Execute(params object[] obj)
        {
            BpEventbus.ActionEvents.OnSelectionIncrementTriggered?.Invoke();
        }
    
        public void Restore(params object[] obj)
        {
            BpEventbus.ActionEvents.OnRestoreSelectionAmount?.Invoke();
        }
    }

}
