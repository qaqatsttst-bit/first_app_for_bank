namespace FirstAppForBank.Application.Services;

public sealed class UpsertCategoryRequest
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; } = true;
}
