using FirstAppForBank.Domain.Enums;

namespace FirstAppForBank.Application.Services;

public sealed class CategoryAdminDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; }
}
