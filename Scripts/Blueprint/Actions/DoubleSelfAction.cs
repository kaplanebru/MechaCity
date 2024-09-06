using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blueprint
{
    public class DoubleSelfAction : IBpAction
    {
   
        public void Execute(params object[] obj)
        {
            Debug.Log("execute bp");
            var selectedTowers = (int[]) obj[0];

        }
        public void Restore(params object[] obj)
        {
            //sonsuza kadar double kalacaksa gerek yok
        }
    }

}
