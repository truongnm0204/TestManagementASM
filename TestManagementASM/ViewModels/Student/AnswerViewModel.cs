using System.ComponentModel;
using System.Runtime.CompilerServices;
using TestManagementASM.Models;

namespace TestManagementASM.ViewModels.Student;

public class AnswerViewModel : INotifyPropertyChanged
{
    private readonly Answer _answer;
    private readonly TakeTestViewModel _parentViewModel;
    private bool _isSelected;

    public AnswerViewModel(Answer answer, TakeTestViewModel parentViewModel)
    {
        _answer = answer;
        _parentViewModel = parentViewModel;
    }

    public int AnswerId => _answer.AnswerId;
    public string AnswerText => _answer.AnswerText;
    public int QuestionId => _answer.QuestionId;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

