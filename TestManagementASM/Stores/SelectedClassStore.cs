using TestManagementASM.Models;

namespace TestManagementASM.Stores;

public class SelectedClassStore
{
    private Class? _selectedClass;

    public Class? SelectedClass
    {
        get => _selectedClass;
        set => _selectedClass = value;
    }
}

