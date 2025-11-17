using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TestManagementASM.Models;
using TestManagementASM.ViewModels.Student;

namespace TestManagementASM.Helpers;

public class QuestionTypeTemplateSelector : DataTemplateSelector
{
    public DataTemplate? SingleChoiceTemplate { get; set; }
    public DataTemplate? MultipleChoiceTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is Answer answer)
        {
            // Find the parent UserControl to get the DataContext (TakeTestViewModel)
            var parent = container;
            while (parent != null && parent is not UserControl)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is FrameworkElement element &&
                element.DataContext is TakeTestViewModel viewModel)
            {
                var currentQuestion = viewModel.CurrentQuestion;
                if (currentQuestion != null)
                {
                    return currentQuestion.QuestionType == "SINGLE"
                        ? SingleChoiceTemplate
                        : MultipleChoiceTemplate;
                }
            }
        }

        // Default to SingleChoiceTemplate if we can't determine
        return SingleChoiceTemplate ?? base.SelectTemplate(item, container);
    }
}

