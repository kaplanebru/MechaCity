using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Blueprint
{
    [CreateAssetMenu(menuName = "Classes/" + nameof(PersonaData), fileName = nameof(PersonaData))]
    public class PersonaData : ScriptableObject
    {
        public PersonaType Type;
        public BpType[] BpTypes;
    }
}
