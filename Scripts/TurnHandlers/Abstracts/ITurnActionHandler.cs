public interface ITurnActionHandler<out TBaseTransferData>
{
    public TBaseTransferData Data { get; }
    
}