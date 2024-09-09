using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public class DoubleSelfAction : IBpAction
    {
   
        public void Execute(params object[] obj)
        {
            
            var selectedTowers = (int[]) obj[0];
            
            Debug.Log("double tower count: " + selectedTowers.Length);
            
            Eventbus.LinkEvents.OnDoubleSelfAction?.Invoke(LinkOperatorType.Double, selectedTowers);

        }
        public void Restore(params object[] obj)
        {
            //sonsuza kadar double kalacaksa gerek yok
        }
    }

}
