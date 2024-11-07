using System.Collections.Generic;
using Enums;

namespace Blueprint
{
    public class PlayerPersonaData //ayrıca bunun save'i alınabilir
    {
        public PersonaData PersonaData;
        public List<BpType> ActiveBlueprints = new(); //eklenip çıkacak
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
        
        public void SetPlayerPersona(PersonaType type)
        {
            Data.PersonaData = PersonaHolder.GetPersona(type);
            SetActiveBlueprints(_otherBpProvider.GetBlueprints(type, 1));
            _bpSlotHolder.Setup(Data.ActiveBlueprints);
        }
        
        private void SetActiveBlueprints(IEnumerable<BpType> otherBps)
        {
            Data.ActiveBlueprints.Clear();
            Data.ActiveBlueprints.AddRange(Data.PersonaData.BpTypes);
            Data.ActiveBlueprints.AddRange(otherBps);
        }
    }
}