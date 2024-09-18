using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Towers;
using Turn;
using UnityEngine;

namespace Blueprint
{
    public class DoubleSelfAction : IBpAction
    {
   
        public void Execute(params object[] obj)
        {
            var selectedTowers = (int[]) obj[0];
            
            var newDouble = new DoubleTower(selectedTowers);
            newDouble.Equalize();
            newDouble.CreateBridge();

            BpEventbus.ActionEvents.OnDoubleSelfAction.Invoke(newDouble);
        }
        public void Restore(params object[] obj)
        {
            //sonsuza kadar(ölene) double kalacaksa gerek yok
        }
    }

}
