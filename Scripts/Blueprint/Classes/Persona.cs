using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public abstract class Persona
    {
        public PersonaType Type;
        public PersonaData Data;
       
        
        public Persona(PersonaData data)
        {
            Data = data;
        }
       
    }
}

