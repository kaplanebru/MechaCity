
using Enums;
using Network;
using UnityEngine;

namespace Blueprint
{
    public class ReverseAction : IBpAction
    {
        public BpType BPType { get; set; } = BpType.Reverse;

        public void Execute(params object[] obj)
        {
            Debug.Log("execute reverse");
            BpEventbus.ActionEvents.OnReverseActionTriggered?.Invoke();
        }
        
        public void Restore(params object[] obj)
        {
            var selectedTower = (int) obj[0];
        }
    }
}
