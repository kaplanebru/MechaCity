using Enums;

namespace Blueprint
{
    public class BpFreeze : BaseBlueprint, IBpActionProcessor<FreezeAction>
    {
        public override BpType Type { get; set; } = BpType.Freeze;
        public FreezeAction BpAction { get; } = new FreezeAction();

        public override void TryTakeAction()
        {
            BpAction.Execute();
        }

        void GetSelectedTower() 
        {
            //TODO: tower selection'ı için ayrı modüler class yaz. Selectionda da, towergroupda da, burda da bu çalışsın.
            //State olarak da çalışabilir.
            //Networklü ve networksüz çalışma opsiyonları olmalı
        }

        void ShowInstructionUI()
        {
            
        }
    }
}
