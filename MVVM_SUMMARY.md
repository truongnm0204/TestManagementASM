# 📋 MVVM SUMMARY - TÓM TẮT TOÀN BỘ

---

## 🎯 MVVM LÀ GÌ?

**MVVM = Model-View-ViewModel**

```
View (XAML)
  ↓ Data Binding
ViewModel (C#)
  ↓ Dependency Injection
Service (C#)
  ↓ Database
Model (Database)
```

---

## 3️⃣ NGUYÊN TẮC VÀNG

### 1. Separation of Concerns
- View: Chỉ hiển thị UI
- ViewModel: Xử lý logic
- Model: Lưu trữ dữ liệu

### 2. Data Binding
- View tự động cập nhật khi ViewModel thay đổi
- Sử dụng `{Binding PropertyName}`

### 3. Dependency Injection
- ViewModel nhận Service qua constructor
- Không tạo Service trực tiếp

---

## 5️⃣ BƯỚC PHÁT TRIỂN

1. **Tạo Model** - Định nghĩa entity
2. **Tạo Service** - Implement business logic
3. **Tạo ViewModel** - Xử lý UI logic
4. **Tạo View** - Thiết kế UI
5. **Kết nối DI** - Inject dependencies

---

## 📖 CÁCH ĐỌC CODE

### 7 Bước:
1. Tìm Entry Point (Button, TextBox)
2. Tìm Command/Property trong ViewModel
3. Đọc Method được gọi
4. Theo dõi Event
5. Đọc Form ViewModel
6. Đọc Service
7. Tóm tắt luồng

---

## 💻 TEMPLATE - COPY & PASTE

### Model
```csharp
public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
}
```

### Service Interface
```csharp
public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<bool> AddAsync(User user);
}
```

### Service Implementation
```csharp
public class UserService : IUserService
{
    private readonly DbContext _context;
    
    public UserService(DbContext context) => _context = context;
    
    public async Task<List<User>> GetAllAsync()
        => await _context.Users.ToListAsync();
    
    public async Task<bool> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
```

### ViewModel Base
```csharp
public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void SetProperty<T>(ref T field, T value, 
        [CallerMemberName] string? name = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
```

### List ViewModel
```csharp
public class UserListViewModel : ViewModelBase
{
    private readonly IUserService _service;
    private ObservableCollection<User> _users = new();
    
    public ObservableCollection<User> Users
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }
    
    public ICommand AddCommand { get; }
    public event Action<UserFormViewModel>? OnShowForm;
    
    public UserListViewModel(IUserService service)
    {
        _service = service;
        AddCommand = new RelayCommand(AddUser);
        _ = LoadAsync();
    }
    
    private async Task LoadAsync()
    {
        var users = await _service.GetAllAsync();
        Users = new ObservableCollection<User>(users);
    }
    
    private void AddUser()
    {
        var formVm = new UserFormViewModel(_service);
        formVm.OnClosed += async () => await LoadAsync();
        OnShowForm?.Invoke(formVm);
    }
}
```

### Form ViewModel
```csharp
public class UserFormViewModel : ViewModelBase
{
    private readonly IUserService _service;
    private User _user = new();
    private string _error = string.Empty;
    
    public User User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }
    
    public string Error
    {
        get => _error;
        set => SetProperty(ref _error, value);
    }
    
    public ICommand SaveCommand { get; }
    public event Action? OnClosed;
    
    public UserFormViewModel(IUserService service)
    {
        _service = service;
        SaveCommand = new RelayCommand(async () => await SaveAsync());
    }
    
    public void InitializeForCreate()
    {
        User = new User();
        Error = string.Empty;
    }
    
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(User.Username))
        {
            Error = "Username không được để trống!";
            return;
        }
        
        await _service.AddAsync(User);
        OnClosed?.Invoke();
    }
}
```

### View (XAML)
```xaml
<UserControl>
    <Grid>
        <DataGrid ItemsSource="{Binding Users}"/>
        <Button Command="{Binding AddCommand}" Content="Thêm"/>
    </Grid>
</UserControl>
```

### Dependency Injection (App.xaml.cs)
```csharp
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        var context = new DbContext();
        var service = new UserService(context);
        var vm = new UserListViewModel(service);
        
        MainWindow = new MainWindow { DataContext = vm };
        MainWindow.Show();
    }
}
```

---

## ✅ CHECKLIST

- [ ] Logic ở ViewModel, không ở code-behind
- [ ] View chỉ chứa XAML
- [ ] ViewModel không biết View
- [ ] Service được inject qua constructor
- [ ] Sử dụng ICommand cho button
- [ ] Sử dụng Data Binding cho property
- [ ] Sử dụng ObservableCollection cho list
- [ ] Sử dụng async/await
- [ ] Có error handling
- [ ] Có validation
- [ ] Có loading indicator
- [ ] Có user feedback

---

## 🐛 DEBUGGING

```csharp
// Thêm debug output
Debug.WriteLine($"Value: {value}");

// Đặt breakpoint (F9)
// Start debugging (F5)
// Step over (F10)
// Step into (F11)
```

---

## 📚 TÀI LIỆU

- **MVVM_GUIDE.md** - Hướng dẫn cơ bản
- **MVVM_GUIDE_PART2.md** - Advanced Patterns
- **MVVM_QUICK_REFERENCE.md** - Tóm tắt nhanh
- **HOW_TO_READ_CODE.md** - Hướng dẫn đọc code
- **README_MVVM.md** - Giới thiệu

---

**Chúc bạn thành công! 🚀**

