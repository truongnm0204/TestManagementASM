using TestManagementASM.Models;

namespace TestManagementASM.Stores;

public class SelectedQuestionStore
{
    private Question? _selectedQuestion;

    public Question? SelectedQuestion
    {
        get => _selectedQuestion;
        set => _selectedQuestion = value;
    }
}

