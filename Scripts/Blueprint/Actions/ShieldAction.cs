using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blueprint
{
    public class ShieldAction: IBpAction
    {
        public void Execute(params object[] obj)
        {
            Debug.Log("execute shield");
            var selectedActors = (uint[]) obj[0];
            //Eventbus.ActorEvents.OnDoubleTowerCreated?.Invoke(selectedActors);
        }
    }
}
