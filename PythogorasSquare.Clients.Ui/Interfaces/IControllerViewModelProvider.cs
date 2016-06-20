namespace PythogorasSquare.Clients.Ui.Interfaces
{
    public interface IControllerViewModelProvider<TController, TViewModel>
    {
        TViewModel GetViewModelFor(TController controller);

        TController GetControllerOf(TViewModel viewModel);
    }
}