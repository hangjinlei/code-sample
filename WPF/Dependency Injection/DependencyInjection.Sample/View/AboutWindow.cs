using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DependencyInjection.Sample.View;

public class AboutWindow : Window
{
    public AboutWindow()
    {
        this.Title = "About";

        var grid = new Grid();

        var stackPanel = new StackPanel()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        var textBlock = new TextBlock()
        {
            FontSize = 40
        };

        var binding = new Binding()
        {
            Source = this,
            Path = new PropertyPath(nameof(Title)),
            Mode = BindingMode.OneWay
        };

        // binding to textBlock Text property
        textBlock.SetBinding(TextBlock.TextProperty, binding);

        stackPanel.Children.Add(textBlock);

        grid.Children.Add(stackPanel);

        this.Content = grid;
    }
}
