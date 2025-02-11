using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;

namespace GylleneDroppen.Api.Attributes;

public class AdminAttribute : AuthorizeAttribute
{
    public AdminAttribute()
    {
        Roles = "Admin";
    }
}