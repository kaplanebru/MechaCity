using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public class BpDouble : BaseBlueprint, IBpActionProcessor<DoubleAction>
    {
        public override BpType Type { get; set; } = BpType.Double;
        public DoubleAction BpAction { get; } = new DoubleAction();
        
        public List<object> IncomingObjects;

        public override void TryTakeAction()
        {
            BpAction.Execute(IncomingObjects);
        }
    }

}

