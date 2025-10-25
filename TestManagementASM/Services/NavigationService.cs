using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.Services;

public class NavigationService : INavigationService
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<Type, ViewModelBase> _viewModelFactory;

    public NavigationService(NavigationStore navigationStore, Func<Type, ViewModelBase> viewModelFactory)
    {
        _navigationStore = navigationStore;
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        var viewModel = _viewModelFactory(typeof(TViewModel));
        _navigationStore.CurrentViewModel = viewModel;
    }

    public void NavigateTo(ViewModelBase viewModel)
    {
        _navigationStore.CurrentViewModel = viewModel;
    }
}
