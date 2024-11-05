using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public class PersonaHolder : MonoBehaviour
    {
        public Dictionary<PersonaType, Persona> personas = new();
        public TypeDataCouple<PersonaType, PersonaData>[] datasByType;

        // void CreatePersonas()
        // {
        //     personas.Add(PersonaType.Jester, new Jester(dataByType.Data));
        //     personas.Add(PersonaType.Fighter, new Fighter());
        //     personas.Add(PersonaType.Defender, new Defender());
        // }
    }
}