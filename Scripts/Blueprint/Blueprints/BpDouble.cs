using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public class BpDouble : BaseBlueprint, IBpActionProcessor<DoubleAction>
    {
        public DoubleAction BpAction { get; }
        public override BpType Type { get; set; }
    }

}

