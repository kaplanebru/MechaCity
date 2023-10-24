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
        private CogData Data;
        private Cogwheel cog;

        private void OnEnable()
        {
            cog = GetComponent<Cogwheel>();
            Data = cog.Data;
            
            ChainEvents.OnCogSetupRequest += Setup;
        }

        void Setup()
        {
            //Data = data;
            print("setup");
            var radius = Data.Radius;
            var scale = Vector3.one;
            scale.x = radius * 2;
            
            if(ChainSpawner.Upwards == ChainEnums.UpAxis.Z)
                scale.z = radius * 2;
            else
                scale.y = radius * 2;

            cog.cogObject.transform.localScale = scale;
            
            //Data.cogObject.transform.localScale = scale;
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
