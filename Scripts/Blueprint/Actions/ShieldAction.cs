using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class ShieldAction: IBpAction
    {
        public void Execute(params object[] obj)
        {
            Debug.Log("execute shield");
            var selectedActors = (uint[]) obj[0];
            var towers = ActorDB.Registry[selectedActors[0]].Towers;

            foreach (var tower in towers)
            {
                tower.VisualSupportedDatas[VisualDataType.Shield].SetDataAndVisuals(tower.Height);
            }
        }
    }
}
