namespace GylleneDroppen.Infrastructure.Settings;

public class AdminSettings
{
    public required bool CreateAdminUser { get; set; }

    public required AdminUserSettings AdminUser { get; set; }
}

public class AdminUserSettings
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}