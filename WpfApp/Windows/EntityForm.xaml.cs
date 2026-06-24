using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp.Data.Models.Attributes;

namespace WpfApp.Windows
{
    /// <summary>
    /// Interaction logic for EntityForm.xaml
    /// </summary>
    public partial class EntityForm : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<TextBox> _textBoxes = new();

        public object Entity { get; private set; }

        public EntityForm(Type entityType, IServiceProvider serviceProvider, object existingEntity = null)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();

            Width = 400;
            Height = 350;
            Title = existingEntity == null ? "Добавление" : "Редактирование";
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Entity = existingEntity ?? Activator.CreateInstance(entityType);
            GenerateForm(entityType);
        }

        private void GenerateForm(Type entityType)
        {
            var properties = entityType.GetProperties()
                .Where(p => p.GetCustomAttribute<PrintableAttribute>(false) != null)
                .ToList();

            var scrollViewer = new ScrollViewer();
            var panel = new StackPanel { Margin = new Thickness(10) };

            foreach (var prop in properties)
            {
                var value = prop.GetValue(Entity)?.ToString() ?? "";

                panel.Children.Add(new TextBlock
                {
                    Text = prop.Name,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 10, 0, 5)
                });

                var textBox = new TextBox
                {
                    Text = value,
                    Tag = prop,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                _textBoxes.Add(textBox);
                panel.Children.Add(textBox);
            }

            // Кнопки
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0)
            };

            var saveBtn = new Button
            {
                Content = "Сохранить",
                Width = 80,
                Margin = new Thickness(0, 0, 10, 0),
                Padding = new Thickness(10, 5, 10, 5)
            };
            saveBtn.Click += (s, e) =>
            {
                if (SaveEntity())
                {
                    DialogResult = true;
                    Close();
                }
            };

            var cancelBtn = new Button
            {
                Content = "Отмена",
                Width = 80,
                Padding = new Thickness(10, 5, 10, 5)
            };
            cancelBtn.Click += (s, e) => { DialogResult = false; Close(); };

            buttonPanel.Children.Add(saveBtn);
            buttonPanel.Children.Add(cancelBtn);
            panel.Children.Add(buttonPanel);

            scrollViewer.Content = panel;
            Content = scrollViewer;
        }

        private bool SaveEntity()
        {
            try
            {
                foreach (var textBox in _textBoxes)
                {
                    if (textBox.Tag is PropertyInfo prop)
                    {
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                        {
                            MessageBox.Show($"Поле {prop.Name} не может быть пустым",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return false;
                        }

                        var convertedValue = Convert.ChangeType(textBox.Text, prop.PropertyType);
                        prop.SetValue(Entity, convertedValue);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
