using System.ComponentModel.DataAnnotations;

namespace PersonalSite.Api.Models;

public sealed record ContactRequest
{
    [Required, MaxLength(120)]
    public string Name { get; init; } = string.Empty;

    [Required, EmailAddress, MaxLength(254)]
    public string Email { get; init; } = string.Empty;

    [Required, MaxLength(2000)]
    public string Message { get; init; } = string.Empty;
}
