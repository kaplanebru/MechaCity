using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public class PlayerPersonaData //ayrıca bunun save'i alınabilir
    {
        public PersonaData PersonaData;
        public PersonaData CommonPersona;
        public List<BpType> ActiveBlueprints = new(); //eklenip çıkacak
        public List<BpType> OtherBpTypes = new();
        public int Fund = 10;
    }
    public class PlayerPersona
    {
        private PlayerPersonaData Data = new();
        private OtherBpProvider _otherBpProvider = new();
        private BPSlotHolder _bpSlotHolder;
        public PlayerPersona(BPSlotHolder bpSlotHolder)
        {
            _bpSlotHolder = bpSlotHolder;
        }

        void Setup(PersonaType type)
        {
            Data.PersonaData = PersonaHolder.GetPersona(type);
            Data.CommonPersona = PersonaHolder.GetPersona(PersonaType.Common);
            Data.OtherBpTypes = _otherBpProvider.GetBlueprints(type, 1).ToList();
        }
        public void SetPlayerPersona(PersonaType type)
        {
            Setup(type);
            SetActiveBlueprints(Data.OtherBpTypes);
            _bpSlotHolder.Setup(Data.ActiveBlueprints);
        }
        
        private void SetActiveBlueprints(IEnumerable<BpType> otherBps)
        {
            Data.ActiveBlueprints.Clear();
            Data.ActiveBlueprints.AddRange(Data.PersonaData.BpTypes);
            Data.ActiveBlueprints.Add(GetRandomCommonBp());
            Data.ActiveBlueprints.AddRange(otherBps);
        }

        private BpType GetRandomCommonBp()
        {
            return Data.CommonPersona.BpTypes.ElementAt(Random.Range(0, Data.CommonPersona.BpTypes.Length));
        }
    }
}