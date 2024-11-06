using System;
using System.Collections.Generic;
using System.Linq;
using Enums;

namespace Blueprint
{
    public class OtherBpProvider
    {
        private static readonly Random _random = new Random();
        private Dictionary<PersonaType, Persona> _personas = new();
        private List<BpType> otherBlueprints = new();

        public OtherBpProvider(Dictionary<PersonaType, Persona> personas)
        {
            _personas = personas;
        }

        public IEnumerable<BpType> GetBlueprints(PersonaType ownType, int amount)
        {
            otherBlueprints.Clear();
            otherBlueprints = _personas
                .Where(p => p.Key != ownType)
                .SelectMany(p => p.Value.Data.BpTypes)
                .ToList();
            
            otherBlueprints = otherBlueprints.OrderBy(_ => _random.Next()).ToList();
            return otherBlueprints.Take(amount);
        }
    }
}