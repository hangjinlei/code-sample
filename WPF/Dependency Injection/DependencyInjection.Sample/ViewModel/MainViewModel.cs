using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DependencyInjection.Sample.Service;
using DependencyInjection.Sample.View;

namespace DependencyInjection.Sample.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly ISayHelloService sayHelloService;
    private readonly IAbstractFactory<AboutWindow> factory;

    public MainViewModel(ISayHelloService sayHelloService, IAbstractFactory<AboutWindow> factory)
    {
        this.sayHelloService = sayHelloService;
        this.factory = factory;
    }

    [ObservableProperty]
    public string name = default!;

    [RelayCommand]
    public void SayHello()
    {
        sayHelloService.SayHello(Name);
    }

    [RelayCommand]
    public void ShowAboutWindow()
    {
        factory.Create().Show();
    }
}
