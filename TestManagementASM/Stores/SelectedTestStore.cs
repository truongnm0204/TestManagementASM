using TestManagementASM.Models;

namespace TestManagementASM.Stores;

public class SelectedTestStore
{
    private Test? _selectedTest;

    public Test? SelectedTest
    {
        get => _selectedTest;
        set => _selectedTest = value;
    }
}

