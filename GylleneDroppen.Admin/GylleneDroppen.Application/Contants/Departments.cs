using GylleneDroppen.Application.Dtos.User;

namespace GylleneDroppen.Application.Contants;

public static class Departments
{
    public static readonly List<DepartmentDto> All =
    [
        new DepartmentDto { Value = "economy", DisplayName = "Ekonomi" },
        new DepartmentDto { Value = "tech", DisplayName = "Teknik" },
        new DepartmentDto { Value = "social_media", DisplayName = "Sociala Medier" }
    ];

    public static string GetDisplayName(string value)
    {
        return All.FirstOrDefault(d => d.Value == value)?.DisplayName ?? value;
    }
}