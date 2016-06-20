namespace PythogorasSquare.Common.Threading
{
    public interface IDeferredOperation
    {
        IOperationDeferral GetDeferral();
    }
}