using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public class PersonaHolder : MonoBehaviour
    {
        public Dictionary<PersonaType, Persona> personas = new();
        public TypeDataCouple<PersonaType, PersonaData>[] dataByTypeSerialized;
        
        private OtherBpProvider _otherBpProvider;
        private Dictionary<PersonaType, PersonaData> dataByType = new();
        
        public Persona GetPersona(PersonaType type) => personas[type];
        public IEnumerable GetOtherBP(PersonaType ownType, int amount) => _otherBpProvider.GetBlueprints(ownType, amount);
        
        private void OnEnable()
        {
            Setup();
        }

        public void Setup()
        {
            SetDataByType();
            CreatePersonas();
            _otherBpProvider = new OtherBpProvider(personas);
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
            personas.Add(PersonaType.Jester, new Jester(dataByType[PersonaType.Jester]));
            personas.Add(PersonaType.Fighter, new Fighter(dataByType[PersonaType.Fighter]));
            personas.Add(PersonaType.Defender, new Defender(dataByType[PersonaType.Defender]));
        }
        
    }
}