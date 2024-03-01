
using Towers;

namespace Turn
{
    public interface ITurnTransferHandler<out TTurnData> where TTurnData : BaseTurnTransferData
    {
        public TTurnData TransferData { get; }

    }
}
