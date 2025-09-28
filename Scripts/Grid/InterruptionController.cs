using System.Collections.Generic;
using System.Linq;

namespace Grid
{
    public class InterruptionController
    {
        private List<InterruptionByActor> interruptionActors = new();
        private GridData Data;
      

        public InterruptionController(GridData data)
        {
            Data = data;
        }

        internal void TryCheckInterruptions(List<uint> linkedActors)
        {
            foreach (var interruption in interruptionActors)
            {
                if (linkedActors.Contains(interruption.Interrupted)) continue;
                if (!linkedActors.All(interruption.Interrupters.Contains)) continue;
                if (linkedActors.Count < interruption.Interrupters.Count) continue;
                //if every item in linkedActors exists in Interrupters
                //with !: if not all items in linkedActors are contained in Interrupters
                
                Eventbus.LinkEvents.OnInterruptionDetected?.Invoke(interruption.Interrupted, interruption.Offset);
                return;
            }
        }
        
        internal void SetInterruptionActors() //her actor yenilendiğinde
        {
            interruptionActors.Clear();
            foreach (var interruptionSlot in Data.interruptions)
            {
                InterruptionByActor interruptionByActor = new();

                interruptionByActor.id = interruptionSlot.id;
                interruptionByActor.Interrupted = RotativeGrid.actorBySlot[interruptionSlot.Interrupted].ID;
                interruptionByActor.Offset = interruptionSlot.Offset;

                foreach (var slot in interruptionSlot.Interrupters)
                {
                    interruptionByActor.Interrupters.Add(RotativeGrid.actorBySlot[slot].ID);
                }
                interruptionActors.Add(interruptionByActor);
            }
        }

       
    }
}