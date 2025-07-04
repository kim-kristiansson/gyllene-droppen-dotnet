using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.Address;

public class CreateAddressRequestDto
{
    [Required(ErrorMessage = "Namn är obligatoriskt.")]
    [MaxLength(100, ErrorMessage = "Namnet får vara högst 100 tecken.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gatuadress är obligatorisk.")]
    [MaxLength(200, ErrorMessage = "Gatuadressen får vara högst 200 tecken.")]
    public string StreetAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Stad är obligatorisk.")]
    [MaxLength(100, ErrorMessage = "Staden får vara högst 100 tecken.")]
    public string City { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "Postnumret får vara högst 20 tecken.")]
    public string? PostalCode { get; set; }

    [MaxLength(500, ErrorMessage = "Beskrivningen får vara högst 500 tecken.")]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}