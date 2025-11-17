using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace TestManagementASM.Views.Teacher;

public partial class QuestionFormView : UserControl
{
    public QuestionFormView()
    {
        InitializeComponent();
    }
}

public class EditModeToTitleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isEditMode)
        {
            return isEditMode ? "Chỉnh sửa câu hỏi" : "Thêm câu hỏi mới";
        }
        return "Câu hỏi";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class EditModeToButtonTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isEditMode)
        {
            return isEditMode ? "Cập nhật" : "Lưu";
        }
        return "Lưu";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

