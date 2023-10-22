using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;


namespace Chain
{
   
    public class CogSetter : MonoBehaviour
    {
        public CogData Data;
        
        void Setup()
        {
            var radius = Data.Radius;
            var scale = cogObject.transform.localScale;
            scale.x = radius * 2;
            
            if(ChainSpawner.Upwards == ChainEnums.UpAxis.Z)
                scale.z = radius * 2;
            else
                scale.y = radius * 2;
            
            cogObject.transform.localScale = scale;
            transform.position += Data.PositionOffset;
            
            //SetHoleSize();
            //ChainEvents.OnCogStart?.Invoke(Data, teeth);
        }
    }

}
