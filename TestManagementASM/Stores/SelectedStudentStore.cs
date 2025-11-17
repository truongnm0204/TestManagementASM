using TestManagementASM.Models;

namespace TestManagementASM.Stores;

public class SelectedStudentStore
{
    private User? _selectedStudent;
    private Class? _selectedClass;

    public User? SelectedStudent
    {
        get => _selectedStudent;
        set => _selectedStudent = value;
    }

    public Class? SelectedClass
    {
        get => _selectedClass;
        set => _selectedClass = value;
    }
}

