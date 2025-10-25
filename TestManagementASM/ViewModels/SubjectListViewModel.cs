using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels;

public class SubjectListViewModel : ViewModelBase
{
    private readonly ISubjectService _subjectService;
    private ObservableCollection<Subject> _subjects = new();
    private Subject? _selectedSubject;
    private string _searchText = string.Empty;
    private bool _isLoading;

    public ObservableCollection<Subject> Subjects
    {
        get => _subjects;
        set => SetProperty(ref _subjects, value);
    }

    public Subject? SelectedSubject
    {
        get => _selectedSubject;
        set => SetProperty(ref _selectedSubject, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            _ = LoadSubjectsAsync();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoadDataCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand RefreshCommand { get; }

    public SubjectListViewModel(ISubjectService subjectService)
    {
        _subjectService = subjectService;

        LoadDataCommand = new RelayCommand(async () => await LoadSubjectsAsync());
        RefreshCommand = new RelayCommand(async () => await LoadSubjectsAsync());
        AddCommand = new RelayCommand(AddSubject);
        EditCommand = new RelayCommand(EditSubject, () => SelectedSubject != null);
        DeleteCommand = new RelayCommand(async () => await DeleteSubjectAsync(), () => SelectedSubject != null);

        _ = LoadSubjectsAsync();
    }

    private async Task LoadSubjectsAsync()
    {
        try
        {
            IsLoading = true;
            var subjects = await _subjectService.GetAllSubjectsAsync();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                subjects = subjects.Where(s =>
                    s.SubjectCode.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    s.SubjectName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            Subjects = new ObservableCollection<Subject>(subjects);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải danh sách môn học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void AddSubject()
    {
        MessageBox.Show("Chức năng thêm môn học sẽ được triển khai sau!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void EditSubject()
    {
        if (SelectedSubject != null)
        {
            MessageBox.Show($"Chức năng sửa môn học '{SelectedSubject.SubjectName}' sẽ được triển khai sau!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private async Task DeleteSubjectAsync()
    {
        if (SelectedSubject == null)
            return;

        var result = MessageBox.Show(
            $"Bạn có chắc chắn muốn xóa môn học '{SelectedSubject.SubjectName}'?",
            "Xác nhận xóa",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                IsLoading = true;
                var success = await _subjectService.DeleteSubjectAsync(SelectedSubject.SubjectId);

                if (success)
                {
                    MessageBox.Show("Xóa môn học thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadSubjectsAsync();
                }
                else
                {
                    MessageBox.Show("Xóa môn học thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa môn học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
