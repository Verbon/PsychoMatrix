namespace PythogorasSquare.Foundation.Interfaces
{
    public interface IEntityControllerFactory<in TEntity, out TController>
    {
        TController CreateFrom(TEntity serviceEntity);
    }
}