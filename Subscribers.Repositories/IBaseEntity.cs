namespace Subscribers.Repositories;
public interface IBaseEntity
{
    Guid Id { get; set; }
    DateTime CreatedDate { get; set; }
    DateTime UpdatedDate { get; set; }
}
