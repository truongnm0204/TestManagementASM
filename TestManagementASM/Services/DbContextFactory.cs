using TestManagementASM.Models;

namespace TestManagementASM.Services;

public class DbContextFactory
{
    public static TestManagementDbContext CreateDbContext()
    {
        return new TestManagementDbContext();
    }
}

