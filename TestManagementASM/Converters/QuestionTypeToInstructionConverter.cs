using System;
using System.Globalization;
using System.Windows.Data;

namespace TestManagementASM.Converters;

public class QuestionTypeToInstructionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string questionType)
        {
            return questionType == "SINGLE" ? "Chọn một đáp án" : "Chọn nhiều đáp án";
        }
        return "Chọn đáp án";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

