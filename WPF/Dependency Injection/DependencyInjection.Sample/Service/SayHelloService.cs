using System.Windows;

namespace DependencyInjection.Sample.Service;

public class SayHelloService : ISayHelloService
{
    public void SayHello(string name) => MessageBox.Show($"Hello {name}");
}
