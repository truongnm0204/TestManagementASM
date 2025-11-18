using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels;

public class ClassListViewModel : ViewModelBase
{
    private readonly IClassService _classService;
    private readonly ISubjectService _subjectService;
    private ObservableCollection<Class> _classes = new();
    private Class? _selectedClass;
    private string _searchText = string.Empty;
    private bool _isLoading;

    public ObservableCollection<Class> Classes
    {
        get => _classes;
        set => SetProperty(ref _classes, value);
    }

    public Class? SelectedClass
    {
        get => _selectedClass;
        set
        {
            SetProperty(ref _selectedClass, value);
            // Trigger CanExecuteChanged for Edit and Delete commands
            _editCommand?.RaiseCanExecuteChanged();
            _deleteCommand?.RaiseCanExecuteChanged();
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            _ = LoadClassesAsync();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoadDataCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }

    public event Action<ClassFormViewModel>? OnShowClassForm;

    private RelayCommand? _editCommand;
    private RelayCommand? _deleteCommand;

    public ClassListViewModel(IClassService classService, ISubjectService subjectService)
    {
        _classService = classService;
        _subjectService = subjectService;

        LoadDataCommand = new RelayCommand(async () => await LoadClassesAsync());
        RefreshCommand = new RelayCommand(async () => await LoadClassesAsync());
        AddCommand = new RelayCommand(AddClass);
        _editCommand = new RelayCommand(EditClass, () => SelectedClass != null);
        _deleteCommand = new RelayCommand(async () => await DeleteClassAsync(), () => SelectedClass != null);
        EditCommand = _editCommand;
        DeleteCommand = _deleteCommand;

        _ = LoadClassesAsync();
    }

    private async Task LoadClassesAsync()
    {
        try
        {
            IsLoading = true;
            var classes = await _classService.GetAllClassesAsync();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                classes = classes.Where(c =>
                    c.ClassName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    c.Subject?.SubjectName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true
                ).ToList();
            }

            Classes = new ObservableCollection<Class>(classes);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải danh sách lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void AddClass()
    {
        var formVm = new ClassFormViewModel(_classService, _subjectService);
        formVm.InitializeForCreate();
        formVm.OnClosed += async () => await LoadClassesAsync();
        OnShowClassForm?.Invoke(formVm);
    }

    private void EditClass()
    {
        if (SelectedClass != null)
        {
            var formVm = new ClassFormViewModel(_classService, _subjectService);
            formVm.InitializeForEdit(SelectedClass);
            formVm.OnClosed += async () => await LoadClassesAsync();
            OnShowClassForm?.Invoke(formVm);
        }
    }

    private async Task DeleteClassAsync()
    {
        if (SelectedClass == null)
            return;

        var result = MessageBox.Show(
            $"Bạn có chắc chắn muốn xóa lớp '{SelectedClass.ClassName}'?",
            "Xác nhận xóa",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                var success = await _classService.DeleteClassAsync(SelectedClass.ClassId);
                if (success)
                {
                    MessageBox.Show("Xóa lớp thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadClassesAsync();
                }
                else
                {
                    MessageBox.Show("Xóa lớp thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

