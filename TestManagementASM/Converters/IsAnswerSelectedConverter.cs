using System;
using System.Globalization;
using System.Windows.Data;
using TestManagementASM.Models;
using TestManagementASM.ViewModels.Student;

namespace TestManagementASM.Converters;

public class IsAnswerSelectedConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 && 
            values[0] is TakeTestViewModel viewModel && 
            values[1] is Answer answer)
        {
            return viewModel.IsAnswerSelected(answer.QuestionId, answer.AnswerId);
        }
        return false;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

