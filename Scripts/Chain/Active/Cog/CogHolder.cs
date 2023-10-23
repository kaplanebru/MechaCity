using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEngine;

namespace Chain
{
    [Serializable]
    public class CogHolder
    {
        public CogData Data;
        public Cogwheel Cog;
        //public Transform CogObject;

        public void SetCogData()
        {
            //Cog.Data = Data;
            Debug.Log("set cog data");
        }
        
    }

}
