namespace BuildingBlocks.Core.Model;

public interface IEntity<T>
{
    public T Id { get; set; }
}