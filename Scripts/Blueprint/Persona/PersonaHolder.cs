using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public class PersonaHolder : MonoBehaviour
    {
        public static Dictionary<PersonaType, PersonaData> Personas = new();
        public static PersonaData GetPersona(PersonaType type) => Personas[type];
        public TypeDataCouple<PersonaType, PersonaData>[] dataByTypeSerialized;
        private Dictionary<PersonaType, PersonaData> dataByType = new();
        
        private void OnEnable()
        {
            Setup();
        }

        public void Setup()
        {
            SetDataByType();
            CreatePersonas();
        }

        void SetDataByType() //TODO: generalize
        {
            foreach (var item in dataByTypeSerialized)
            {
                dataByType.Add(item.Type, item.Data);
            }
        }
        
        void CreatePersonas()
        {
            Personas.Add(PersonaType.Jester, dataByType[PersonaType.Jester]);
            Personas.Add(PersonaType.Fighter,dataByType[PersonaType.Fighter]);
            Personas.Add(PersonaType.Defender, dataByType[PersonaType.Defender]);
        }
        
    }
}