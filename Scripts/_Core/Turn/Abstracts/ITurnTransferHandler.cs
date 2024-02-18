

namespace Turn
{
    public interface ITurnTransferHandler<out TTurnData>
    {
        public TTurnData TransferData { get; }
    }
}
