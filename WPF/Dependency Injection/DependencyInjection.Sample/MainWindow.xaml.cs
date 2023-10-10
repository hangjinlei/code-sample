using DependencyInjection.Sample.ViewModel;
using System.Windows;

namespace DependencyInjection.Sample;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel mainViewModel)
    {
        InitializeComponent();
        this.DataContext = mainViewModel;
    }
}