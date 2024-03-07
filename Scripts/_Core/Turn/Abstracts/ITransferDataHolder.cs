
using Enums;
using Towers;

namespace Turn
{
    public interface ITransferDataHolder<out TTurnData> where TTurnData : BaseTurnTransferData
    {
        public TTurnData TransferData { get; }

    }
}
