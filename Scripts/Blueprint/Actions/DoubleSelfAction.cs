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
            var selectedTowers = (int[]) obj[0];
            
            var newDouble = new DoubleTowerPhysical(selectedTowers);
            newDouble.Equalize();
            newDouble.CreateBridge();
            
            Eventbus.ActorEvents.OnDoubleTowerCreated?.Invoke(selectedTowers);
            
        }
        public void Restore(params object[] obj)
        {
            //sonsuza kadar(ölene) double kalacaksa gerek yok
        }
    }

}
