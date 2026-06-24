using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLib.models;
using ClassLib.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.Data.Models.Attributes;
using WpfApp.mvvm;

namespace WpfApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _vm;

    public MainWindow()
    {
        MainWindowViewModel mainWindowViewModel = App.ServiceProvider.GetRequiredService<MainWindowViewModel>();
        _vm = mainWindowViewModel;
        InitializeComponent();
        DataContext = _vm;

        _vm.OnEntityChanged += GenerateDataGridColumns;

        if (EntitiesComboBox.Items.Count > 0)
        {
            EntitiesComboBox.SelectedIndex = 0;
        }
    }

    private void EntitiesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            _vm.LoadData(EntitiesComboBox.SelectedItem as string ?? _vm.EntityTypes.First().Key);

    private void SearchButton_Click(object sender, RoutedEventArgs e) =>
        _vm.SearchById(IdTextBox.Text);

    private void AddButton_Click(object sender, RoutedEventArgs e) =>
        _vm.ExecuteAdd();

    private void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int id)
        {
            _vm.ExecuteUpdate(id);
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int id)
        {
            _vm.ExecuteDelete(id);
        }
    }

    private void GenerateDataGridColumns(Type entityType)
    {
        ItemsDataGrid.Columns.Clear();

        var properties = entityType.GetProperties()
            .Where(p => p.CanRead && p.GetCustomAttribute<PrintableAttribute>(false) != null)
            .ToList();

        foreach (var prop in properties)
        {
            var column = new DataGridTextColumn
            {
                Header = prop.Name,
                Binding = new Binding(prop.Name),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };
            ItemsDataGrid.Columns.Add(column);
        }

        var buttonColumn = new DataGridTemplateColumn
        {
            Header = "Действия",
            Width = new DataGridLength(200)
        };

        var factory = new FrameworkElementFactory(typeof(StackPanel));
        factory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
        factory.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Center);

        var updateButton = new FrameworkElementFactory(typeof(Button));
        updateButton.SetValue(Button.ContentProperty, "изменить");
        updateButton.SetValue(Button.TagProperty, new Binding("Id"));
        updateButton.SetValue(Button.MarginProperty, new Thickness(2));
        updateButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(UpdateButton_Click));
        factory.AppendChild(updateButton);

        var deleteButton = new FrameworkElementFactory(typeof(Button));
        deleteButton.SetValue(Button.ContentProperty, "удалить");
        deleteButton.SetValue(Button.TagProperty, new Binding("Id"));
        deleteButton.SetValue(Button.MarginProperty, new Thickness(2));
        deleteButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(DeleteButton_Click));
        factory.AppendChild(deleteButton);

        buttonColumn.CellTemplate = new DataTemplate { VisualTree = factory };
        ItemsDataGrid.Columns.Add(buttonColumn);
    }
}