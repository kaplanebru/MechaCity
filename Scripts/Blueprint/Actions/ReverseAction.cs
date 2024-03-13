
using Network;
using UnityEngine;

namespace Blueprint
{
    public class ReverseAction : IBpAction
    {
        public void Execute(params object[] obj)
        {
            BpEventbus.ActionEvents.OnReverseActionTriggered?.Invoke();
        }
        
        public void Restore(params object[] obj)
        {
            var selectedTower = (int) obj[0];
          
        }
    }
}
