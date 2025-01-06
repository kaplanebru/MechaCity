using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using Towers;

using UnityEngine;

namespace Blueprint
{
    public class DoubleSelfAction : IBpAction
    {
        public void Execute(params object[] obj)
        {
            Debug.Log("execute double self");
            var selectedActors = (uint[]) obj[0];
            selectedActors = selectedActors.OrderBy(a => a).ToArray();
            
            var newDouble = new DoubleTowerPhysical(selectedActors);
            newDouble.Equalize();
            newDouble.CreateBridge();
            
            Debug.Log("new double");
            
            Eventbus.ActorEvents.OnDoubleTowerCreated?.Invoke(selectedActors);
            
        }
        public void Restore(params object[] obj)
        {
            //sonsuza kadar(ölene) double kalacaksa gerek yok
        }
    }

}
