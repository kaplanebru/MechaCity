using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Network;
using UnityEngine;

namespace Blueprint
{
    public class BpHolder : MonoBehaviour
    {
        public Dictionary<BpType, BaseBlueprint> Blueprints = new();
        //public Dictionary<BpType, IBpAction> BpActions = new(); //alternative
    
        private void OnEnable()
        {
            NetworkEventbus.BlueprintEvents.OnBpSelected += ExecuteBpAction;
        }

        private void ExecuteBpAction(BpType type)
        {
            Blueprints[type].BpAction.Execute();
        }

        void CreateBlueprints()
        {
            Blueprints.Add(BpType.Reverse, new BpReverse());
            Blueprints.Add(BpType.Freeze, new BpFreeze());
        }

        private void OnDisable()
        {
            NetworkEventbus.BlueprintEvents.OnBpSelected -= ExecuteBpAction;
        }
    }


    public abstract class BaseBlueprint: IBpActionProcessor<IBpAction>
    {
        public abstract BpType Type { get; set; }
        public IBpAction BpAction { get; }
 
    }

    public interface IBpActionProcessor<out TAction> where TAction : IBpAction
    {
        public TAction BpAction { get; }
        public BpType Type { get; set; }
    }


    public interface IBpAction
    {
        public void Execute();
    }
}

