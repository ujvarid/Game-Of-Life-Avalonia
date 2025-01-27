using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System;
using Simulation.Model;
using System.Linq;

namespace Simulation.Avalonia.ViewModels;

public partial class SimulationViewModel : ViewModelBase
{
    #region Fields

    private SimulationModel _model = null!;
    private bool _isEnabled;
    private string _simulationButtonText = "Start";

    #endregion

    #region Properties

    public int TableSize => _model.TableSize;   
    public int Interval => _model.Interval;
    public bool IsSimulationOn
    {
        get => _model.IsSimulationOn;
        set
        {
            _model.IsSimulationOn = value;
            OnPropertyChanged(nameof(IsSimulationOn));
        }
    }
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            OnPropertyChanged(nameof(IsEnabled));
        }
    }
    public string SimulationButtonText
    {
        get => _simulationButtonText;
        set
        {
            _simulationButtonText = value;
            OnPropertyChanged(nameof(SimulationButtonText));
        }
    }
    public RelayCommand? StartStopSimulationCommand { get; set; }
    public RelayCommand? FasterCommand { get; set; }
    public RelayCommand? SlowerCommand { get; set; }
    public RelayCommand? ClearCommand { get; set; }

    public ObservableCollection<SimulationField> Fields { get; set; }

    #endregion

    #region Events

    public event EventHandler? StartStopSimulation;
    public event EventHandler? Faster;
    public event EventHandler? Slower;
    public event EventHandler? Clear;

    #endregion

    #region Ctor

    public SimulationViewModel(SimulationModel model)
    {
        _model = model;
        _model.FieldChanged += Model_FieldChanged;
        IsEnabled = true;
        StartStopSimulationCommand = new RelayCommand(OnStartStopSimulation);
        FasterCommand = new RelayCommand(OnFaster);
        SlowerCommand = new RelayCommand(OnSlower);
        ClearCommand = new RelayCommand(OnClear);
        GenerateTable();
    }

    #endregion

    #region Private Game Methods

    private void OnStartStopSimulation()
    {
        StartStopSimulation?.Invoke(this, EventArgs.Empty);
    }

    private void OnFaster()
    {
        Faster?.Invoke(this, EventArgs.Empty);
    }

    private void OnSlower()
    {
        Slower?.Invoke(this, EventArgs.Empty);
    }

    private void OnClear()
    {
        Clear?.Invoke(this, EventArgs.Empty);
    }

    private void GenerateTable()
    {
        Fields = new ObservableCollection<SimulationField>();
        for (int i = 0; i < TableSize; i++)
        {
            for (int j = 0; j < TableSize; j++)
            {
                Fields.Add(new SimulationField
                {
                    X = i,
                    Y = j,
                    Alive = _model[i, j],
                    StepCommand = new RelayCommand<Tuple<int, int>>(param =>
                    {
                        if (param is Tuple<int, int> xy)
                        {
                            StepGame(xy.Item1, xy.Item2);
                        }
                    })
                });
            }
        }
    }

    #endregion

    #region Public Game Methods

    public void StepGame(int x, int y)
    {
        _model.Step(x, y);
    }

    #endregion

    #region Model Event Handlers

    private void Model_FieldChanged(object? sender, SimulationFieldEventArgs e)
    {
        SimulationField field = Fields.Single(f => f.X == e.X && f.Y == e.Y);
        field.Alive = _model[field.X, field.Y];
    }

    #endregion
}
