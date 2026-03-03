namespace OrderSystem.Domain.Entities;

public abstract class Entity
{
    public required Guid Id { get; init; }
    public required DateTimeOffset CreationDate { get; init; }
    public required DateTimeOffset UpdateDate { get; set; }
    public required bool Active { get; set; }

    public Entity() { }

    public void RenewUpdateDate()
    {
        UpdateDate = DateTimeOffset.UtcNow;
    }

    public void SetActive()
    {
        Active = true;
    }

    public void Disable()
    {
        Active = false;
    }
}
