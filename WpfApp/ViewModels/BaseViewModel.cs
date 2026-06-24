using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.Repositories.Interfaces;
using System.Windows.Input;
using System.Windows;
using ClassLib.models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp.mvvm;

public abstract class BaseViewModel
{
    protected readonly IServiceProvider _serviceProvider;

    public BaseViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
}