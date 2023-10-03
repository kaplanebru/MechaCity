

namespace Turn
{
    public interface ITurnActionHandler<out TTurnData>
    {
        public TTurnData TransferData { get; }
    }
}
