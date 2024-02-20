using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Network;
using UnityEngine;

namespace Blueprint
{
    public class BpHolder 
    {
        public Dictionary<BpType, BaseBlueprint> AllBlueprints = new();

        public List<BpType> activeBlueprints = new();
        //public Dictionary<BpType, IBpAction> BpActions = new(); //alternative
    
        public void Subscribe()
        {
            NetworkEventbus.BlueprintEvents.OnBpSelected += ExecuteBpAction;
        }
        public void Initialize()
        {
            CreateBlueprints();
        }
        void CreateBlueprints() //Burası ortadaki kısımla ilgili
        {
            AllBlueprints.Add(BpType.Reverse, new BpReverse());
            AllBlueprints.Add(BpType.Freeze, new BpFreeze());
            AllBlueprints.Add(BpType.Double, new BpDouble());
        }
        
        private void ExecuteBpAction(BpType type)
        {
            AllBlueprints[type].BpAction.Execute();
        }

        public void GetActiveBlueprints() 
        {
            for (int i = 0; i < 3; i++) //TODO: Temp
            {
                activeBlueprints.Add(AllBlueprints.Keys.ElementAt(i));
            }
         
        }

        public void Unsubscribe()
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

