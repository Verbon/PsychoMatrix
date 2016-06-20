namespace PythogorasSquare.Clients.Ui.Interfaces
{
    public interface IControllerViewModelFactory<in TController, out TViewModel>
    {
        TViewModel CreateFrom(TController controller);
    }
}