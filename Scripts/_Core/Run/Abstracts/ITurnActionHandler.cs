

namespace Core
{
    public interface ITurnActionHandler<out TTurnData>
    {
        public TTurnData TransferData { get; }
    }
}
