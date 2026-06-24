using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using ClassLib.models;
using ClassLib.Repositories;
using ClassLib.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.Windows;

namespace WpfApp.mvvm;

public class MainWindowViewModel : BaseViewModel, INotifyPropertyChanged
{
    private Type _currentEntityType;
    private object _currentRepository;
    private ObservableCollection<object> _items = new();
    private string _searchIdText;

    // Словарь для ComboBox
    public Dictionary<string, Type> EntityTypes { get; } = new()
    {
        { "Сотрудники", typeof(Employee) },
        { "Контрагенты", typeof(Counterparty) },
        { "Заказы", typeof(Order) }
    };

    // Коллекция для отображения в DataGrid
    public ObservableCollection<object> Items
    {
        get => _items;
        set
        {
            _items = value;
            OnPropertyChanged();
        }
    }

    // Текст для поиска
    public string SearchIdText
    {
        get => _searchIdText;
        set
        {
            _searchIdText = value;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public MainWindowViewModel(IServiceProvider serviceProvider): base(serviceProvider)
    {
        SearchIdText = "";
    }

    public void LoadData(string entityName)
    {
        if (!EntityTypes.TryGetValue(entityName, out var entityType)) return;

        _currentEntityType = entityType; 
        _currentRepository = _serviceProvider.GetRequiredService(typeof(IRepository<>).MakeGenericType(entityType));
        
        var method = _currentRepository.GetType().GetMethod("GetAll");
        var data = method?.Invoke(_currentRepository, null) as IEnumerable<object>;

        Items.Clear();
        if (data != null)
        {
            foreach (var item in data)
            {
                Items.Add(item);
            }
        }

        OnEntityChanged?.Invoke(entityType);
    }

    public event Action<Type> OnEntityChanged;

    public void SearchById(string idText)
    {
        if (string.IsNullOrWhiteSpace(idText))
        {
            LoadData(GetCurrentEntityName());
            return;
        }

        if (!int.TryParse(idText, out int id))
        {
            MessageBox.Show("Введите корректный ID", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var method = _currentRepository.GetType().GetMethod("GetById");
            var result = method?.Invoke(_currentRepository, new object[] { id });

            Items.Clear();
            if (result != null)
            {
                Items.Add(result);
            }
            else
            {
                MessageBox.Show($"Запись с ID {id} не найдена", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void ExecuteAdd()
    {
        var form = new EntityForm(_currentEntityType, _serviceProvider);
        if (form.ShowDialog() == true)
        {
            var addMethod = _currentRepository.GetType().GetMethod("Add");
            addMethod?.Invoke(_currentRepository, new[] { form.Entity });
            LoadData(GetCurrentEntityName());
        }
    }

    public void ExecuteUpdate(int id)
    {
        var item = Items.FirstOrDefault(x =>
        {
            var prop = x.GetType().GetProperty("Id");
            return prop != null && prop.GetValue(x) is int i && i == id;
        });

        if (item == null) return;

        var form = new EntityForm(_currentEntityType, _serviceProvider, item);
        if (form.ShowDialog() == true)
        {
            var entityToUpdate = _currentRepository.GetType().GetMethod("GetById").Invoke(_currentRepository, new object[] { id });
            var updateMethod = _currentRepository.GetType().GetMethod("Update");
            updateMethod?.Invoke(_currentRepository, new[] { entityToUpdate });
            LoadData(GetCurrentEntityName());
        }
    }

    public void ExecuteDelete(int id)
    {
        if (MessageBox.Show($"Удалить запись с ID {id}?", "Подтверждение",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            try
            {
                var entityToDelete = _currentRepository.GetType().GetMethod("GetById").Invoke(_currentRepository, new object[] { id });
                var deleteMethod = _currentRepository.GetType().GetMethod("Delete");
                deleteMethod?.Invoke(_currentRepository, new object[] { entityToDelete });
                LoadData(GetCurrentEntityName());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private string GetCurrentEntityName()
    {
        return EntityTypes.First(x => x.Value == _currentEntityType).Key;
    }
}