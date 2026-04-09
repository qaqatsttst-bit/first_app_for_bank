namespace FirstAppForBank.Domain.Models;

public sealed class Category
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Service> Services { get; init; } = new List<Service>();
}
