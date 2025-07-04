using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.Department;

public class UpdateDepartmentRequestDto
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Namn är obligatoriskt.")]
    [MaxLength(100, ErrorMessage = "Namnet får vara högst 100 tecken.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Beskrivningen får vara högst 500 tecken.")]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}