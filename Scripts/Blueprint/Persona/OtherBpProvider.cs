using System;
using System.Collections.Generic;
using System.Linq;
using Enums;

namespace Blueprint
{
    public class OtherBpProvider
    {
        private static readonly Random _random = new Random();
        
        private List<BpType> otherBlueprints = new();
        
        public IEnumerable<BpType> GetBlueprints(PersonaType ownType, int amount)
        {
            otherBlueprints.Clear();
            otherBlueprints = PersonaHolder.Personas
                .Where(p => p.Key != ownType)
                .SelectMany(p => p.Value.BpTypes)
                .ToList();
            
            otherBlueprints = otherBlueprints.OrderBy(_ => _random.Next()).ToList();
            return otherBlueprints.Take(amount);
        }
    }
}