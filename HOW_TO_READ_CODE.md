# 📖 HƯỚNG DẪN ĐỌC CODE - CÁCH HIỂU CHỨC NĂNG TRONG MVVM PROJECT

---

## 🎯 NGUYÊN TẮC CƠ BẢN

**Khi đọc code MVVM, luôn bắt đầu từ UI (View) và đi xuống (ViewModel → Service → Model)**

```
View (XAML)
    ↓ (Binding)
ViewModel (C#)
    ↓ (Dependency)
Service (C#)
    ↓ (Database)
Model (Database)
```

---

## 📚 PHƯƠNG PHÁP ĐỌC CODE - 7 BƯỚC

### Bước 1: Tìm Entry Point (Điểm vào)

**Bắt đầu từ UI - tìm button, textbox, hoặc event**

```xaml
<!-- UserListView.xaml -->
<Button Content="➕ Thêm" Command="{Binding AddCommand}"/>
<Button Content="✏️ Sửa" Command="{Binding EditCommand}"/>
<Button Content="🗑️ Xóa" Command="{Binding DeleteCommand}"/>
```

**Ghi chú:** 
- `Command="{Binding AddCommand}"` → Tìm `AddCommand` trong ViewModel
- `ItemsSource="{Binding Users}"` → Tìm `Users` property trong ViewModel

### Bước 2: Tìm Command/Property trong ViewModel

**Tìm ViewModel tương ứng với View**

```csharp
// UserListViewModel.cs
public ICommand AddCommand { get; }
public ICommand EditCommand { get; }
public ICommand DeleteCommand { get; }
public ObservableCollection<User> Users { get; set; }

public UserListViewModel(IUserService userService)
{
    AddCommand = new RelayCommand(AddUser);
    EditCommand = new RelayCommand(EditUser);
    DeleteCommand = new RelayCommand(async () => await DeleteUserAsync());
}
```

**Ghi chú:**
- `AddCommand` → Gọi method `AddUser()`
- `EditCommand` → Gọi method `EditUser()`
- `DeleteCommand` → Gọi method `DeleteUserAsync()`

### Bước 3: Đọc Method được gọi

**Đọc method mà Command gọi**

```csharp
private void AddUser()
{
    // Bước 1: Tạo form ViewModel
    var formVm = new UserFormViewModel(_userService);
    
    // Bước 2: Initialize for create
    formVm.InitializeForCreate();
    
    // Bước 3: Subscribe to OnClosed event
    formVm.OnClosed += async () => await LoadUsersAsync();
    
    // Bước 4: Trigger event để hiển thị form
    OnShowUserForm?.Invoke(formVm);
}
```

**Ghi chú:**
- `new UserFormViewModel(_userService)` → Tạo form ViewModel
- `formVm.InitializeForCreate()` → Chuẩn bị form cho create mode
- `OnShowUserForm?.Invoke(formVm)` → Trigger event để hiển thị form

### Bước 4: Theo dõi Event

**Tìm nơi subscribe event này**

```csharp
// MainViewModel.cs hoặc parent ViewModel
userListVm.OnShowUserForm += (formVm) =>
{
    // Hiển thị form modal
    var dialog = new UserFormView { DataContext = formVm };
    dialog.ShowDialog();
};
```

**Ghi chú:**
- Event được trigger → Form được hiển thị
- User nhập dữ liệu → Form ViewModel nhận dữ liệu

### Bước 5: Đọc Form ViewModel

**Đọc logic xử lý trong form**

```csharp
// UserFormViewModel.cs
public void InitializeForCreate()
{
    IsEditMode = false;
    User = new User { Status = 1 };
    ErrorMessage = string.Empty;
}

private async Task SaveAsync()
{
    // Validation
    if (string.IsNullOrWhiteSpace(User.Username))
    {
        ErrorMessage = "Username không được để trống!";
        return;
    }
    
    // Check uniqueness
    var isUnique = await _userService.IsUsernameUniqueAsync(User.Username);
    if (!isUnique)
    {
        ErrorMessage = "Username đã tồn tại!";
        return;
    }
    
    // Save to database
    await _userService.AddUserAsync(User);
    
    // Close form
    OnClosed?.Invoke();
}
```

**Ghi chú:**
- `InitializeForCreate()` → Chuẩn bị form trống
- `SaveAsync()` → Validate → Save → Close

### Bước 6: Đọc Service

**Đọc logic lưu trữ dữ liệu**

```csharp
// UserService.cs
public async Task<bool> AddUserAsync(User user)
{
    // Hash password
    user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
    
    // Add to context
    _context.Users.Add(user);
    
    // Save to database
    await _context.SaveChangesAsync();
    
    return true;
}

public async Task<bool> IsUsernameUniqueAsync(string username)
{
    // Check if username exists
    return !await _context.Users.AnyAsync(u => u.Username == username);
}
```

**Ghi chú:**
- `AddUserAsync()` → Hash password → Add to DB
- `IsUsernameUniqueAsync()` → Check if username exists

### Bước 7: Tóm tắt luồng

**Vẽ sơ đồ luồng từ trên xuống**

```
1. User nhấn "➕ Thêm" button
   ↓
2. AddCommand execute → AddUser() method
   ↓
3. Tạo UserFormViewModel
   ↓
4. InitializeForCreate() → Form trống
   ↓
5. Trigger OnShowUserForm event → Hiển thị form
   ↓
6. User nhập username, password, full name
   ↓
7. User nhấn "Lưu" button
   ↓
8. SaveCommand execute → SaveAsync() method
   ↓
9. Validate dữ liệu
   ↓
10. Gọi _userService.IsUsernameUniqueAsync(username)
    ↓
11. Gọi _userService.AddUserAsync(User)
    ↓
12. Service hash password
    ↓
13. Service lưu vào database
    ↓
14. Trigger OnClosed event
    ↓
15. Form đóng
    ↓
16. Trigger LoadUsersAsync() → Refresh user list
```

---

## 🔍 CÁCH TÌM KIẾM TRONG VISUAL STUDIO

| Phím tắt | Chức năng |
|----------|----------|
| `Ctrl + F` | Tìm trong file hiện tại |
| `Ctrl + Shift + F` | Tìm trong toàn project |
| `Ctrl + G` | Đi đến dòng số |
| `F12` | Go to definition |
| `Ctrl + -` | Go back |
| `Ctrl + Shift + -` | Go forward |
| `Ctrl + K, Ctrl + C` | Comment |
| `Ctrl + K, Ctrl + U` | Uncomment |

---

## 💡 TIPS ĐỌC CODE HIỆU QUẢ

### 1. Sử dụng Breakpoints

```csharp
// Đặt breakpoint tại dòng này (F9)
private void AddUser()
{
    Debug.WriteLine("AddUser called");  // ← Breakpoint ở đây
    var formVm = new UserFormViewModel(_userService);
}
```

**Cách debug:**
- F9: Đặt breakpoint
- F5: Start debugging
- F10: Step over (bỏ qua)
- F11: Step into (vào trong)
- Shift+F11: Step out (thoát ra)

### 2. Sử dụng Debug.WriteLine

```csharp
private async Task SaveAsync()
{
    Debug.WriteLine("SaveAsync started");
    
    if (string.IsNullOrWhiteSpace(User.Username))
    {
        Debug.WriteLine("Username is empty");
        ErrorMessage = "Username không được để trống!";
        return;
    }
    
    Debug.WriteLine($"Saving user: {User.Username}");
    await _userService.AddUserAsync(User);
    
    Debug.WriteLine("SaveAsync completed");
    OnClosed?.Invoke();
}
```

**Xem output:**
- Debug → Windows → Output (Ctrl + Alt + O)

### 3. Vẽ sơ đồ

**Sử dụng Mermaid hoặc draw.io để vẽ sơ đồ luồng**

```
graph TD
    A[User nhấn Thêm] --> B[AddCommand execute]
    B --> C[Tạo UserFormViewModel]
    C --> D[Hiển thị form]
    D --> E[User nhập dữ liệu]
    E --> F[User nhấn Lưu]
    F --> G[SaveCommand execute]
    G --> H[Validate dữ liệu]
    H --> I[Gọi Service]
    I --> J[Lưu vào database]
    J --> K[Refresh list]
```

### 4. Đọc từ dưới lên

**Nếu không hiểu từ trên xuống, hãy đọc từ dưới lên**

```
Database (Model)
    ↑
Service (Xử lý dữ liệu)
    ↑
ViewModel (Xử lý logic)
    ↑
View (UI)
```

---

## 📝 TEMPLATE - GỢI Ý CÁCH GỌI CHỨC NĂNG

### Khi gặp chức năng mới, hãy hỏi:

1. **Entry Point là gì?**
   - Button? TextBox? Event?

2. **Command/Property là gì?**
   - Tìm trong ViewModel

3. **Method được gọi là gì?**
   - Đọc method

4. **Event được trigger là gì?**
   - Tìm nơi subscribe

5. **Service được gọi là gì?**
   - Đọc Service

6. **Database operation là gì?**
   - Đọc Model

7. **Luồng hoàn chỉnh là gì?**
   - Vẽ sơ đồ

---

**Chúc bạn đọc code hiệu quả! 🚀**

