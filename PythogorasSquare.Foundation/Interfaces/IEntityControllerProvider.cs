namespace PythogorasSquare.Foundation.Interfaces
{
    public interface IEntityControllerProvider<TEntity, TController>
    {
        TController GetControllerFor(TEntity entity);

        TEntity GetEntityOf(TController controller);
    }
}