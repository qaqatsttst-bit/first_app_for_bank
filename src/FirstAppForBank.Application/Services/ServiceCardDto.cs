namespace FirstAppForBank.Application.Services;

public sealed record ServiceCardDto(
    Guid Id,
    string Name,
    string Category,
    string Status,
    string Criticality,
    string? Owner);
