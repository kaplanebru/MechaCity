
using Enums;
using Towers;

namespace Turn
{
    public interface ITurnTransferHandler<out TTurnData> where TTurnData : BaseTurnTransferData
    {
        public TTurnData TransferData { get; }

    }

    // public interface ITurnType<out TType> where TType : TurnStateType
    // {
    //     
    // }
}
