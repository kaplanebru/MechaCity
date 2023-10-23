using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class CogSetter : MonoBehaviour
    {
        public CogData Data;

        private void OnEnable()
        {
            ChainEvents.OnCogSetupRequest += Setup;
        }

        void Setup(CogData data)
        {
            //Data = data;
            print("setup");
            var radius = Data.Radius;
            var scale = Data.cogObject.transform.localScale;
            scale.x = radius * 2;
            
            if(ChainSpawner.Upwards == ChainEnums.UpAxis.Z)
                scale.z = radius * 2;
            else
                scale.y = radius * 2;
            
            Data.cogObject.transform.localScale = scale;
            //transform.position += Data.PositionOffset;
            
            //SetHoleSize();
            //ChainEvents.OnCogStart?.Invoke(Data, teeth);
        }

        private void OnDisable()
        {
            ChainEvents.OnCogSetupRequest -= Setup;
        }
    }

}
