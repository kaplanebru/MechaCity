
using Network;
using UnityEngine;

namespace Blueprint
{
    public class ReverseAction : IBpAction
    {
        public void Execute()
        {
            BpEventbus.ActionEvents.OnReverseActionTriggered?.Invoke();
        }
    }
}
