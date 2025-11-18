# 📚 HƯỚNG DẪN MVVM PATTERN - PHẦN 2: ADVANCED PATTERNS & BEST PRACTICES

---

## 5️⃣ ADVANCED PATTERNS - CÁC MẪU NÂNG CAO

### Pattern 1: Modal Dialog Communication

**Vấn đề:** Làm sao để form modal giao tiếp với parent ViewModel?

**Giải pháp:** Sử dụng Events

```csharp
// UserListViewModel.cs
public event Action<UserFormViewModel>? OnShowUserForm;

private void AddUser()
{
    var formVm = new UserFormViewModel(_userService);
    formVm.InitializeForCreate();
    formVm.OnClosed += async () => await LoadUsersAsync();
    OnShowUserForm?.Invoke(formVm);
}

// UserFormViewModel.cs
public event Action? OnClosed;

private async Task SaveAsync()
{
    await _userService.AddUserAsync(User);
    OnClosed?.Invoke();
}
```

### Pattern 2: Search & Filter

**Vấn đề:** Làm sao để search/filter hiệu quả?

**Giải pháp:** Sử dụng PropertyChanged event

```csharp
public class ClassListViewModel : ViewModelBase
{
    private string _searchText = string.Empty;
    
    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            _ = LoadClassesAsync();
        }
    }
    
    private async Task LoadClassesAsync()
    {
        var classes = await _classService.GetAllClassesAsync();
        
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            classes = classes.Where(c =>
                c.ClassName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }
        
        Classes = new ObservableCollection<Class>(classes);
    }
}
```

### Pattern 3: Loading State

**Vấn đề:** Làm sao để hiển thị loading indicator?

**Giải pháp:** Sử dụng IsLoading property

```csharp
public class ClassListViewModel : ViewModelBase
{
    private bool _isLoading;
    
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }
    
    private async Task LoadClassesAsync()
    {
        try
        {
            IsLoading = true;
            var classes = await _classService.GetAllClassesAsync();
            Classes = new ObservableCollection<Class>(classes);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
```

```xaml
<Border Background="#80000000" 
        Visibility="{Binding IsLoading, Converter={StaticResource BoolToVisibilityConverter}}">
    <TextBlock Text="Đang tải dữ liệu..." Foreground="White"/>
</Border>
```

### Pattern 4: Validation

**Vấn đề:** Làm sao để validate dữ liệu?

**Giải pháp:** Sử dụng ErrorMessage property

```csharp
public class UserFormViewModel : ViewModelBase
{
    private string _errorMessage = string.Empty;
    
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }
    
    private async Task SaveAsync()
    {
        ErrorMessage = string.Empty;
        
        if (string.IsNullOrWhiteSpace(User.Username))
        {
            ErrorMessage = "Username không được để trống!";
            return;
        }
        
        if (User.Username.Length < 3)
        {
            ErrorMessage = "Username phải có ít nhất 3 ký tự!";
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
}
```

### Pattern 5: Command with Parameter

**Vấn đề:** Làm sao để truyền parameter vào Command?

**Giải pháp:** Sử dụng RelayCommand với parameter

```csharp
public class ClassListViewModel : ViewModelBase
{
    public ICommand DeleteCommand { get; }
    
    public ClassListViewModel(IClassService classService)
    {
        DeleteCommand = new RelayCommand(
            param => DeleteClass((int)param!),
            param => SelectedClass != null
        );
    }
    
    private void DeleteClass(int classId)
    {
        var result = MessageBox.Show(
            "Bạn có chắc chắn muốn xóa?",
            "Xác nhận",
            MessageBoxButton.YesNo
        );
        
        if (result == MessageBoxResult.Yes)
        {
            _ = DeleteClassAsync(classId);
        }
    }
}
```

```xaml
<Button Command="{Binding DeleteCommand}" 
        CommandParameter="{Binding SelectedClass.ClassId}"
        Content="Xóa"/>
```

## 6️⃣ CÁCH ĐỌC CODE - ADVANCED

### 📖 Ví dụ 2: Đọc chức năng "Xóa lớp học"

**Bước 1: Tìm Button**
```xaml
<Button Command="{Binding DeleteCommand}" CommandParameter="{Binding SelectedClass.ClassId}"/>
```

**Bước 2: Tìm DeleteCommand**
```csharp
public ICommand DeleteCommand { get; }

public ClassListViewModel(IClassService classService)
{
    DeleteCommand = new RelayCommand(
        param => DeleteClass((int)param!),
        () => SelectedClass != null
    );
}
```

**Bước 3: Đọc DeleteClass method**
```csharp
private void DeleteClass(int classId)
{
    var result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButton.YesNo);
    
    if (result == MessageBoxResult.Yes)
    {
        _ = DeleteClassAsync(classId);
    }
}
```

**Bước 4: Đọc DeleteClassAsync method**
```csharp
private async Task DeleteClassAsync(int classId)
{
    try
    {
        var success = await _classService.DeleteClassAsync(classId);
        if (success)
        {
            MessageBox.Show("Xóa thành công!");
            await LoadClassesAsync();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Lỗi: {ex.Message}");
    }
}
```

**Bước 5: Đọc Service**
```csharp
public async Task<bool> DeleteClassAsync(int classId)
{
    var @class = await _context.Classes.FindAsync(classId);
    if (@class == null) return false;
    
    _context.Classes.Remove(@class);
    await _context.SaveChangesAsync();
    return true;
}
```

**Bước 6: Tóm tắt luồng**
```
1. User chọn class, nhấn "Xóa"
   ↓
2. DeleteCommand execute với classId parameter
   ↓
3. Hiển thị confirmation dialog
   ↓
4. Nếu user chọn "Yes" → DeleteClassAsync(classId)
   ↓
5. Service xóa class từ database
   ↓
6. Refresh class list
   ↓
7. Hiển thị success message
```

## 7️⃣ DEBUGGING TIPS

### 🐛 Cách debug MVVM

**1. Kiểm tra Binding**
```csharp
public string Username
{
    get => _username;
    set
    {
        Debug.WriteLine($"Username changed: {value}");
        SetProperty(ref _username, value);
    }
}
```

**2. Kiểm tra Command Execute**
```csharp
private void AddUser()
{
    Debug.WriteLine("AddUser command executed");
}
```

**3. Kiểm tra Event Trigger**
```csharp
private async Task SaveAsync()
{
    Debug.WriteLine("SaveAsync started");
    await _userService.AddUserAsync(User);
    Debug.WriteLine("SaveAsync completed");
    OnClosed?.Invoke();
}
```

**4. Sử dụng Breakpoints**
- F9: Đặt breakpoint
- F5: Start debugging
- F10: Step over
- F11: Step into
- Shift+F11: Step out

## 8️⃣ BEST PRACTICES

✅ **LÀM:**
- Tách biệt View, ViewModel, Model
- Sử dụng Dependency Injection
- Sử dụng Data Binding
- Viết Unit Tests cho ViewModel
- Sử dụng async/await cho operations
- Có error handling (try-catch)
- Có validation trước khi save
- Có loading indicator cho long operations
- Có success/error messages
- Code có comments giải thích
- Tên biến/method rõ ràng, dễ hiểu

❌ **KHÔNG LÀM:**
- Viết logic trong code-behind
- Tạo Service trực tiếp trong ViewModel
- Gọi database từ View
- Sử dụng static classes
- Bỏ qua validation
- Không handle exceptions
- Không có loading state
- Không có user feedback

## 9️⃣ CHECKLIST - KHI VIẾT MVVM CODE

- [ ] Tất cả logic ở ViewModel, không ở code-behind
- [ ] View chỉ chứa XAML, không có C# logic
- [ ] ViewModel không biết View tồn tại
- [ ] Service được inject qua constructor
- [ ] Sử dụng ICommand cho button actions
- [ ] Sử dụng Data Binding cho properties
- [ ] Sử dụng ObservableCollection cho lists
- [ ] Sử dụng async/await cho database operations
- [ ] Có error handling (try-catch)
- [ ] Có validation trước khi save
- [ ] Có loading indicator cho long operations
- [ ] Có success/error messages
- [ ] Code có comments giải thích
- [ ] Tên biến/method rõ ràng, dễ hiểu

---

**Chúc bạn thành công với MVVM! 🚀**

