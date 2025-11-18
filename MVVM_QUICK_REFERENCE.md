# 🚀 MVVM QUICK REFERENCE - TÓM TẮT NHANH

## 📋 MVVM STRUCTURE

```
Project/
├── Models/
│   ├── User.cs
│   ├── Class.cs
│   └── Subject.cs
│
├── Services/
│   ├── Interfaces/
│   │   ├── IUserService.cs
│   │   ├── IClassService.cs
│   │   └── ISubjectService.cs
│   ├── UserService.cs
│   ├── ClassService.cs
│   └── SubjectService.cs
│
├── ViewModels/
│   ├── ViewModelBase.cs (base class)
│   ├── UserListViewModel.cs
│   ├── UserFormViewModel.cs
│   ├── ClassListViewModel.cs
│   ├── ClassFormViewModel.cs
│   └── MainViewModel.cs
│
├── Views/
│   ├── UserListView.xaml
│   ├── UserFormView.xaml
│   ├── ClassListView.xaml
│   ├── ClassFormView.xaml
│   └── MainWindow.xaml
│
└── App.xaml.cs (Dependency Injection)
```

## 🔧 TEMPLATE - TẠONHANH MVVM COMPONENT

### 1. Model
```csharp
public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
}
```

### 2. Service Interface
```csharp
public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<bool> AddAsync(User user);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
}
```

### 3. Service Implementation
```csharp
public class UserService : IUserService
{
    private readonly TestManagementDbContext _context;
    
    public UserService(TestManagementDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
    
    public async Task<bool> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
```

### 4. ViewModel Base
```csharp
public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
```

### 5. List ViewModel
```csharp
public class UserListViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private ObservableCollection<User> _users = new();
    private User? _selectedUser;
    
    public ObservableCollection<User> Users
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }
    
    public User? SelectedUser
    {
        get => _selectedUser;
        set => SetProperty(ref _selectedUser, value);
    }
    
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    
    public event Action<UserFormViewModel>? OnShowUserForm;
    
    public UserListViewModel(IUserService userService)
    {
        _userService = userService;
        AddCommand = new RelayCommand(AddUser);
        EditCommand = new RelayCommand(EditUser);
        DeleteCommand = new RelayCommand(async () => await DeleteUserAsync());
        
        _ = LoadUsersAsync();
    }
    
    private async Task LoadUsersAsync()
    {
        var users = await _userService.GetAllAsync();
        Users = new ObservableCollection<User>(users);
    }
    
    private void AddUser()
    {
        var formVm = new UserFormViewModel(_userService);
        formVm.InitializeForCreate();
        formVm.OnClosed += async () => await LoadUsersAsync();
        OnShowUserForm?.Invoke(formVm);
    }
    
    private void EditUser()
    {
        if (SelectedUser != null)
        {
            var formVm = new UserFormViewModel(_userService);
            formVm.InitializeForEdit(SelectedUser);
            formVm.OnClosed += async () => await LoadUsersAsync();
            OnShowUserForm?.Invoke(formVm);
        }
    }
    
    private async Task DeleteUserAsync()
    {
        if (SelectedUser == null) return;
        
        var result = MessageBox.Show($"Xóa {SelectedUser.Username}?", "Xác nhận", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
            await _userService.DeleteAsync(SelectedUser.UserId);
            await LoadUsersAsync();
        }
    }
}
```

### 6. Form ViewModel
```csharp
public class UserFormViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private User _user = new();
    private string _errorMessage = string.Empty;
    private bool _isEditMode;
    
    public User User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }
    
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }
    
    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetProperty(ref _isEditMode, value);
    }
    
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public event Action? OnClosed;
    
    public UserFormViewModel(IUserService userService)
    {
        _userService = userService;
        SaveCommand = new RelayCommand(async () => await SaveAsync());
        CancelCommand = new RelayCommand(() => OnClosed?.Invoke());
    }
    
    public void InitializeForCreate()
    {
        IsEditMode = false;
        User = new User { Status = 1 };
        ErrorMessage = string.Empty;
    }
    
    public void InitializeForEdit(User user)
    {
        IsEditMode = true;
        User = new User
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName
        };
        ErrorMessage = string.Empty;
    }
    
    private async Task SaveAsync()
    {
        ErrorMessage = string.Empty;
        
        if (string.IsNullOrWhiteSpace(User.Username))
        {
            ErrorMessage = "Username không được để trống!";
            return;
        }
        
        if (IsEditMode)
        {
            await _userService.UpdateAsync(User);
        }
        else
        {
            await _userService.AddAsync(User);
        }
        
        MessageBox.Show("Lưu thành công!");
        OnClosed?.Invoke();
    }
}
```

### 7. View (XAML)
```xaml
<UserControl>
    <Grid>
        <DataGrid ItemsSource="{Binding Users}" SelectedItem="{Binding SelectedUser}"/>
        
        <StackPanel Orientation="Horizontal">
            <Button Command="{Binding AddCommand}" Content="Thêm"/>
            <Button Command="{Binding EditCommand}" Content="Sửa"/>
            <Button Command="{Binding DeleteCommand}" Content="Xóa"/>
        </StackPanel>
    </Grid>
</UserControl>
```

### 8. Dependency Injection (App.xaml.cs)
```csharp
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        var dbContext = new TestManagementDbContext();
        var userService = new UserService(dbContext);
        var userListVm = new UserListViewModel(userService);
        
        MainWindow = new MainWindow { DataContext = userListVm };
        MainWindow.Show();
    }
}
```

## 🎯 WORKFLOW - QUY TRÌNH PHÁT TRIỂN

1. **Tạo Model** → Định nghĩa entity
2. **Tạo Service Interface** → Định nghĩa contract
3. **Tạo Service Implementation** → Implement logic
4. **Tạo ViewModel** → Xử lý UI logic
5. **Tạo View** → Thiết kế UI
6. **Kết nối DI** → Inject dependencies
7. **Test** → Kiểm tra chức năng

## 💡 TIPS

- **Luôn sử dụng async/await** cho database operations
- **Luôn validate dữ liệu** trước khi save
- **Luôn có error handling** (try-catch)
- **Luôn có user feedback** (MessageBox, loading indicator)
- **Luôn tách biệt concerns** (View, ViewModel, Model)
- **Luôn sử dụng Dependency Injection**
- **Luôn sử dụng ObservableCollection** cho lists
- **Luôn sử dụng ICommand** cho button actions

---

**Tham khảo MVVM_GUIDE.md và MVVM_GUIDE_PART2.md để hiểu chi tiết hơn!**

