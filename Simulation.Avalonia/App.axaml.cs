using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using Simulation.Avalonia.ViewModels;
using Simulation.Avalonia.Views;
using System;
using Simulation.Model;

namespace Simulation.Avalonia;

public partial class App : Application
{
    #region Fields

    private SimulationModel _model = null!;
    private SimulationViewModel _viewModel = null!;
    const int TABLESIZE = 20;

    #endregion

    #region Application Methods

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        _model = new SimulationModel(new SimulationTimerInheritence(), TABLESIZE);

        _viewModel = new SimulationViewModel(_model);
        _viewModel.StartStopSimulation += ViewModel_StartStopSimulation;
        _viewModel.Faster += ViewModel_Faster;
        _viewModel.Slower += ViewModel_Slower;
        _viewModel.Clear += ViewModel_Clear;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _viewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = _viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    #endregion

    #region ViewModel Event Handlers

    private void ViewModel_StartStopSimulation(object? sender, EventArgs eventArgs)
    {
        if (_viewModel.IsSimulationOn)
        {
            _viewModel.IsEnabled = true;
            _model.StopSimulation();
            _viewModel.IsSimulationOn = false;
            _viewModel.SimulationButtonText = "Start";
        }
        else
        {
            _viewModel.IsEnabled = false;
            _model.StartSimulation();
            _viewModel.IsSimulationOn = true;
            _viewModel.SimulationButtonText = "Stop";
        }
    }

    private void ViewModel_Faster(object? sender, EventArgs eventArgs)
    {
        if (_viewModel.Interval <= 100) return;

        _model.ChangeInterval(_viewModel.Interval - 100);
    }

    private void ViewModel_Slower(object? sender, EventArgs eventArgs)
    {
        _model.ChangeInterval(_viewModel.Interval + 100);
    }

    private void ViewModel_Clear(object? sender, EventArgs eventArgs)
    {
        _model.Clear();
    }

    #endregion
}
