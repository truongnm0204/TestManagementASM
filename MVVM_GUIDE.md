# 📚 HƯỚNG DẪN MVVM PATTERN - TƯ DUY & TIẾP CẬN

**Tài liệu này dành cho lập trình viên muốn hiểu sâu về MVVM pattern, cách tư duy khi thiết kế project, và cách đọc code hiệu quả.**

---

## 1️⃣ MVVM LÀ GÌ?

**MVVM = Model-View-ViewModel**

```
┌─────────────────────────────────────────────────────┐
│                    USER INTERFACE                    │
│                      (View)                          │
│  - XAML files (.xaml)                               │
│  - Hiển thị dữ liệu cho người dùng                  │
│  - Nhận input từ người dùng                         │
└────────────────────┬────────────────────────────────┘
                     │ Data Binding (2-way)
                     ↓
┌─────────────────────────────────────────────────────┐
│              BUSINESS LOGIC LAYER                    │
│                 (ViewModel)                          │
│  - C# classes (.cs)                                 │
│  - Xử lý logic, commands, properties                │
│  - Không biết View tồn tại                          │
│  - Không biết Database tồn tại                      │
└────────────────────┬────────────────────────────────┘
                     │ Dependency Injection
                     ↓
┌─────────────────────────────────────────────────────┐
│              DATA ACCESS LAYER                       │
│                  (Model)                             │
│  - Database entities                                │
│  - Services (UserService, ClassService, etc)        │
│  - Business rules                                   │
└─────────────────────────────────────────────────────┘
```

## 2️⃣ TƯ DUY MVVM - 3 NGUYÊN TẮC VÀNG

### ✅ Nguyên Tắc 1: SEPARATION OF CONCERNS

**Mỗi layer chỉ làm 1 việc:**

| Layer | Trách Nhiệm | Không Làm |
|-------|-----------|----------|
| **View** | Hiển thị UI | Không xử lý logic, không gọi DB |
| **ViewModel** | Xử lý logic | Không biết View, không gọi DB trực tiếp |
| **Model** | Lưu trữ dữ liệu | Không biết View, không xử lý UI logic |

**❌ SAI - Logic ở View:**
```csharp
private void Button_Click(object sender, RoutedEventArgs e)
{
    var user = new User { Username = UsernameTextBox.Text };
    _dbContext.Users.Add(user);
    _dbContext.SaveChanges();
}
```

**✅ ĐÚNG - Logic ở ViewModel:**
```csharp
public ICommand AddUserCommand { get; }

private async Task AddUserAsync()
{
    await _userService.AddUserAsync(User);
}
```

### ✅ Nguyên Tắc 2: DATA BINDING

**View tự động cập nhật khi ViewModel thay đổi:**

```xaml
<TextBox Text="{Binding Username, UpdateSourceTrigger=PropertyChanged}"/>
<Button Command="{Binding SaveCommand}" Content="Lưu"/>
```

```csharp
public string Username
{
    get => _username;
    set => SetProperty(ref _username, value);
}
```

### ✅ Nguyên Tắc 3: DEPENDENCY INJECTION

**ViewModel nhận Service qua constructor:**

```csharp
// ❌ SAI
private IUserService _userService = new UserService();

// ✅ ĐÚNG
public UserListViewModel(IUserService userService)
{
    _userService = userService;
}
```

## 3️⃣ TIẾP CẬN MVVM - 5 BƯỚC

### Bước 1: Xác định chức năng

```
Yêu cầu: Tạo chức năng "Thêm người dùng"
- Nhập username, password, full name
- Kiểm tra username không trùng
- Lưu vào database
- Hiển thị thông báo thành công
```

### Bước 2: Thiết kế Model

```csharp
public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
}
```

### Bước 3: Tạo Service

```csharp
public interface IUserService
{
    Task<bool> AddUserAsync(User user);
    Task<bool> IsUsernameUniqueAsync(string username);
}

public class UserService : IUserService
{
    public async Task<bool> AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
```

### Bước 4: Tạo ViewModel

```csharp
public class UserFormViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private User _user = new();
    
    public ICommand SaveCommand { get; }
    public event Action? OnClosed;
    
    public UserFormViewModel(IUserService userService)
    {
        _userService = userService;
        SaveCommand = new RelayCommand(async () => await SaveAsync());
    }
    
    private async Task SaveAsync()
    {
        await _userService.AddUserAsync(User);
        OnClosed?.Invoke();
    }
}
```

### Bước 5: Tạo View

```xaml
<TextBox Text="{Binding User.Username}"/>
<Button Command="{Binding SaveCommand}" Content="Lưu"/>
```

## 4️⃣ CÁCH ĐỌC CODE - HƯỚNG DẪN TỪNG BƯỚC

### 📖 Ví dụ: Đọc chức năng "Thêm người dùng"

**Bước 1: Tìm Entry Point**
```xaml
<Button Content="➕ Thêm" Command="{Binding AddCommand}"/>
```

**Bước 2: Theo dõi Command**
```csharp
public ICommand AddCommand { get; }

public UserListViewModel(IUserService userService)
{
    AddCommand = new RelayCommand(AddUser);
}
```

**Bước 3: Đọc Method**
```csharp
private void AddUser()
{
    var formVm = new UserFormViewModel(_userService);
    formVm.InitializeForCreate();
    formVm.OnClosed += async () => await LoadUsersAsync();
    OnShowUserForm?.Invoke(formVm);
}
```

**Bước 4: Theo dõi Event**
```csharp
userListVm.OnShowUserForm += (formVm) =>
{
    ShowModalDialog(formVm);
};
```

**Bước 5: Đọc Form ViewModel**
```csharp
private async Task SaveAsync()
{
    if (string.IsNullOrWhiteSpace(User.Username))
    {
        ErrorMessage = "Username không được để trống!";
        return;
    }
    
    var isUnique = await _userService.IsUsernameUniqueAsync(User.Username);
    if (!isUnique)
    {
        ErrorMessage = "Username đã tồn tại!";
        return;
    }
    
    await _userService.AddUserAsync(User);
    OnClosed?.Invoke();
}
```

**Bước 6: Đọc Service**
```csharp
public async Task<bool> AddUserAsync(User user)
{
    user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
    _context.Users.Add(user);
    await _context.SaveChangesAsync();
    return true;
}
```

**Bước 7: Tóm tắt luồng**
```
1. User nhấn "➕ Thêm" button
   ↓
2. AddCommand execute → AddUser() method
   ↓
3. Tạo UserFormViewModel, initialize for create
   ↓
4. Trigger OnShowUserForm event → Hiển thị form
   ↓
5. User nhập dữ liệu, nhấn "Lưu"
   ↓
6. SaveCommand execute → SaveAsync() method
   ↓
7. Validate dữ liệu
   ↓
8. Gọi _userService.AddUserAsync(User)
   ↓
9. Service hash password, lưu vào database
   ↓
10. Trigger OnClosed event → Refresh user list
```

---

**Xem file MVVM_GUIDE_PART2.md để tiếp tục với Advanced Patterns, Debugging Tips, và Best Practices**

