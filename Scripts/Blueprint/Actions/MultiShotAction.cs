using System.Collections;
using System.Collections.Generic;
using Blueprint;
using UnityEngine;

namespace Blueprint
{
    public class MultiShotAction : IBpAction
    {
        public void Execute(params object[] obj)
        {
            var selectedTower = ((int[]) obj[0])[0];
        }
    }

}
