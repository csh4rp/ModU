namespace ModU.Abstract.Domain;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}