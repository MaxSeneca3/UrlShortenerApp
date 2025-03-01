namespace DataAccess.Interfaces;

public interface IEntity<TKey>
{
    TKey Id { get; set; } 
}